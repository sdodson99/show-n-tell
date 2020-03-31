using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Extensions;
using ShowNTell.API.Models.Requests;
using ShowNTell.API.Models.Responses;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagePostsController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly IRandomImagePostService _randomImagePostService;
        private readonly ISearchService _searchService;
        private readonly IImageStorage _imageStorage;
        private readonly IMapper _mapper;
        private readonly ILogger<ImagePostsController> _logger;

        public ImagePostsController(IImagePostService imagePostService, 
            IRandomImagePostService randomImagePostService, 
            ISearchService searchService,
            IImageStorage imageStorage, 
            IMapper mapper,
            ILogger<ImagePostsController> logger)
        {
            _imagePostService = imagePostService;
            _randomImagePostService = randomImagePostService;
            _searchService = searchService;
            _imageStorage = imageStorage;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery(Name = "search")] string searchQuery)
        {
            IEnumerable<ImagePost> searchResult = await _searchService.SearchImagePosts(searchQuery);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(searchResult));
        }

        [HttpGet]
        [Route("random")]
        public async Task<IActionResult> GetRandom()
        {
            ImagePost randomPost = await _randomImagePostService.GetRandom();

            if(randomPost == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ImagePostResponse>(randomPost));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            ImagePost post = await _imagePostService.GetById(id);

            return Ok(_mapper.Map<ImagePostResponse>(post));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateImagePostRequest imagePostRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the user making the request.
            User user = HttpContext.GetUser();

            // Store image file.
            IFormFile image = imagePostRequest.Image;
            string imageUri = await _imageStorage.SaveImage(image.OpenReadStream(), Path.GetExtension(image.FileName));

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

            newImagePost = await _imagePostService.Create(newImagePost);

            return Ok(_mapper.Map<ImagePostResponse>(newImagePost));
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateImagePostRequest imagePostRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the user making the request.
            User user = HttpContext.GetUser();

            // Check if user owns the post.
            bool userOwnsPost = await _imagePostService.IsAuthor(id, user.Email);

            if(!userOwnsPost) 
            {
                return Forbid();
            }

            // Update image database record.
            ImagePost updatedImagePost = await _imagePostService.Update(id, imagePostRequest.Description);

            return Ok(_mapper.Map<ImagePostResponse>(updatedImagePost));
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the user making the request.
            User user = HttpContext.GetUser();

            // Find the image to delete.
            ImagePost imageToDelete = await _imagePostService.GetById(id);
            if(imageToDelete == null)
            {
                return NotFound();
            }

            // Check if user does not own image.
            if(imageToDelete.UserEmail != user.Email) 
            {
                return Forbid();
            }

            // Delete the image record.
            if(!await _imagePostService.Delete(id))
            {
                return NotFound();
            }

            // Delete the image from storage.
            if (!await _imageStorage.DeleteImage(imageToDelete.ImageUri))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
