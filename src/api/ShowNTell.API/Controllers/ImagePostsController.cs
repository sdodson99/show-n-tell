﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Authorization;
using ShowNTell.API.Models.Notifications.ImagePosts;
using ShowNTell.API.Models.Requests;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.API.Services.ImageOptimizations;
using ShowNTell.API.Services.Notifications;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("imageposts")]
    public class ImagePostsController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly IRandomImagePostService _randomImagePostService;
        private readonly ISearchService _searchService;
        private readonly IImageOptimizationService _imageOptimizationService;
        private readonly IImageStorage _imageStorage;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ImagePostsController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ImagePostsController(IImagePostService imagePostService,
            IRandomImagePostService randomImagePostService,
            ISearchService searchService,
            IImageStorage imageStorage,
            IImageOptimizationService imageOptimizationService,
            IMapper mapper, ILogger<ImagePostsController> logger, ICurrentUserService currentUserService, INotificationService notificationService)
        {
            _imagePostService = imagePostService;
            _randomImagePostService = randomImagePostService;
            _searchService = searchService;
            _imageStorage = imageStorage;
            _imageOptimizationService = imageOptimizationService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get all image posts or search for image posts with a search query.
        /// </summary>
        /// <param name="search">The query to search for.</param>
        /// <returns>All image posts or the list of image posts matching the search query.</returns>
        /// <response code="200">Returns all image posts or the list of image posts matching the search query.</response>
        /// <response code="403">Forbidden.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.READ_ACCESS)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagePostResponse>>> Search([FromQuery(Name = "search")] string search)
        {
            _logger.LogInformation("Received image post search request.");

            if(search == null)
            {
                search = string.Empty;
            }

            _logger.LogInformation("Search: '{0}'", search);
            
            IEnumerable<ImagePost> searchResult = await _searchService.SearchImagePosts(search);

            _logger.LogInformation("Successfully retrieved {0} image post(s) matching '{1}'.", searchResult.Count(), search);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(searchResult));
        }

        /// <summary>
        /// Get a random image post.
        /// </summary>
        /// <returns>A random image post.</returns>
        /// <response code="200">Returns a random image post.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">No image posts are available.</response>
        [Produces("application/json")]
        [Authorize(PolicyName.READ_ACCESS)]
        [HttpGet("random")]
        public async Task<ActionResult<ImagePostResponse>> GetRandom()
        {
            _logger.LogInformation("Received random image post request.");
            
            ImagePost randomPost = await _randomImagePostService.GetRandom();

            if(randomPost == null)
            {
                _logger.LogError("No image posts are available to retrieve.");
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved random image post with an id of {0}.", randomPost.Id);

            return Ok(_mapper.Map<ImagePostResponse>(randomPost));
        }

        /// <summary>
        /// Get an image post by id.
        /// </summary>
        /// <param name="id">The id of the image post.</param>
        /// <returns>The image post with the id.</returns>
        /// <response code="200">Returns the image post with the id.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Image post does not exist.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.READ_ACCESS)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ImagePostResponse>> GetById(int id)
        {
            _logger.LogInformation("Received image post get by id request.");
            _logger.LogInformation("Image post id: {0}", id);

            ImagePost post = await _imagePostService.GetById(id);

            if(post == null)
            {
                _logger.LogError("Image post with id {0} does not exist.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved image post with an id of {0}.", post.Id);

            return Ok(_mapper.Map<ImagePostResponse>(post));
        }

        /// <summary>
        /// Create a new image post.
        /// </summary>
        /// <param name="imagePostRequest">The image post to create.</param>
        /// <returns>The created image post.</returns>
        /// <response code="201">Returns the created image post.</response>
        /// <response code="400">Failed to create image post.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [ProducesResponseType(typeof(ImagePostResponse), StatusCodes.Status201Created)]
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPost]
        public async Task<ActionResult<ImagePostResponse>> Create([FromForm] CreateImagePostRequest imagePostRequest)
        {
            _logger.LogInformation("Received image post create request.");

            // Get the user making the request.
            User user = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", user.Email);

            IFormFile image = imagePostRequest.Image;

            // Optimize image.
            _logger.LogInformation("Optimizing image.");
            Stream imageStream = _imageOptimizationService.Optimize(image);

            // Store image file.
            _logger.LogInformation("Saving image file with filename '{0}'.", image.FileName);
            string imageUri = await _imageStorage.SaveImage(imageStream, Path.GetExtension(image.FileName));
            _logger.LogInformation("Successfully saved image file at location '{0}'.", imageUri);

            // Save image database record.
            ImagePost newImagePost = new ImagePost()
            {
                UserEmail = user.Email,
                Description = imagePostRequest.Description,
                ImageUri = imageUri,
                DateCreated = DateTime.Now,
                Tags = imagePostRequest.Tags
                    .Select(content => new ImagePostTag()
                    {
                        Tag = new Tag()
                        {
                            Content = content
                        }
                    }).ToList()
            };

            _logger.LogInformation("Creating image post.");
            newImagePost = await _imagePostService.Create(newImagePost);
            newImagePost.User = user;

            // Map to response object.
            ImagePostResponse imagePostResponse = _mapper.Map<ImagePostResponse>(newImagePost);

            // Publish event.
            _logger.LogInformation("Publishing image post create notification.");
            await _notificationService.Publish(new ImagePostCreatedNotification(imagePostResponse));

            _logger.LogInformation("Successfully created image post with id {0}.", newImagePost.Id);

            return Created($"/imageposts/{newImagePost.Id}", imagePostResponse);
        }

        /// <summary>
        /// Update an image post.
        /// </summary>
        /// <param name="id">The id of the image post to update.</param>
        /// <param name="imagePostRequest">The updated image post values.</param>
        /// <returns>The updated image post.</returns>
        /// <response code="200">Returns the updated image post.</response>
        /// <response code="400">Failed to update image post.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Image post does not exist.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ImagePostResponse>> Update(int id, [FromBody] UpdateImagePostRequest imagePostRequest)
        {
            _logger.LogInformation("Received image post update request.");

            // Get the user making the request.
            User user = _currentUserService.GetCurrentUser(HttpContext);

            _logger.LogInformation("Requesting user email: {0}", user.Email);
            _logger.LogInformation("Image post id: {0}", id);

            // Check if user owns the post.
            _logger.LogInformation("Verifying user owns image post.");
            bool userOwnsPost = await _imagePostService.IsAuthor(id, user.Email);
            if(!userOwnsPost) 
            {
                _logger.LogError("User '{0}' does not own image post with id {1}.", user.Email, id);
                return Forbid();
            }

            try
            {
                // Update database image post.
                _logger.LogInformation("Updating image post with id {0}.", id);
                ImagePost updatedImagePost = await _imagePostService.Update(id, 
                    imagePostRequest.Description, 
                    imagePostRequest.Tags.Select(t => new Tag() { Content = t }));
                updatedImagePost.User = user;

                ImagePostResponse imagePostResponse = _mapper.Map<ImagePostResponse>(updatedImagePost);

                // Publish event.
                _logger.LogInformation("Publishing image post updated notification.");
                await _notificationService.Publish(new ImagePostUpdatedNotification(imagePostResponse));
                
                _logger.LogInformation("Successfully updated image post with id {0}.", id);

                return Ok(imagePostResponse);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError("Image post with id {0} does not exist.", id);
                return NotFound();
            }
        }

        /// <summary>
        /// Delete an image post by id.
        /// </summary>
        /// <param name="id">The id of the image post.</param>
        /// <response code="204">Successfully deleted image post.</response>
        /// <response code="400">Failed to delete stored image post.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Image post does not exist.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Received image post delete request.");
           
            // Get the user making the request.
            User user = _currentUserService.GetCurrentUser(HttpContext);

            _logger.LogInformation("Requesting user email: {0}", user.Email);
            _logger.LogInformation("Image post id: {0}", id);

            // Find the image to delete.
            ImagePost imageToDelete = await _imagePostService.GetById(id);
            if(imageToDelete == null)
            {
                _logger.LogError("Image post with id {0} does not exist.", id);
                return NotFound();
            }

            // Check if user does not own image.
            if(imageToDelete.UserEmail != user.Email) 
            {
                _logger.LogError("User '{0}' does not own image post with id {1}", user.Email, id);
                return Forbid();
            }

            // Delete the image record.
            if(!await _imagePostService.Delete(id))
            {
                _logger.LogError("Image post with id {0} does not exist.", id);
                return NotFound();
            }

            // Delete the image from storage.
            if (!await _imageStorage.DeleteImage(imageToDelete.ImageUri))
            {
                _logger.LogError("Failed to delete stored image at '{0}'.", imageToDelete.ImageUri);
                return BadRequest();
            }

            // Publish event.
            _logger.LogInformation("Publishing image post deleted notification.");
            await _notificationService.Publish(new ImagePostDeletedNotification(id));

            _logger.LogInformation("Successfully deleted image post with id {0}.", id);

            return NoContent();
        }
    }
}
