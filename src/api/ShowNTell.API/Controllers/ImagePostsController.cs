using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Extensions;
using ShowNTell.API.Models.Requests;
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
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<ImagePostsController> _logger;

        public ImagePostsController(IImagePostService imagePostService, IRandomImagePostService randomImagePostService, 
            IImageStorage imageStorage, 
            ILogger<ImagePostsController> logger)
        {
            _imagePostService = imagePostService;
            _randomImagePostService = randomImagePostService;
            _imageStorage = imageStorage;
            _logger = logger;
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

            return Ok(randomPost);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _imagePostService.GetById(id));
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
                DateCreated = DateTime.Now
            };

            newImagePost = await _imagePostService.Create(newImagePost);

            return Ok(newImagePost);
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
            ImagePost updateImagePost = await _imagePostService.UpdateDescription(id, imagePostRequest.Description);

            return Ok(updateImagePost);
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
