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
using ShowNTell.Domain.Services.ImageSavers;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagePostsController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly IRandomImagePostService _randomImagePostService;
        private readonly IImageSaver _imageSaver;
        private readonly ILogger<ImagePostsController> _logger;

        public ImagePostsController(IImagePostService imagePostService, IRandomImagePostService randomImagePostService, 
            IImageSaver imageSaver, 
            ILogger<ImagePostsController> logger)
        {
            _imagePostService = imagePostService;
            _randomImagePostService = randomImagePostService;
            _imageSaver = imageSaver;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllForUser()
        {
            User currentUser = HttpContext.GetUser();

            IEnumerable<ImagePost> posts = await _imagePostService.GetAllByUserEmail(currentUser.Email);

            return Ok(posts);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            string imageUri = await _imageSaver.SaveImage(image.OpenReadStream(), Path.GetExtension(image.FileName));

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
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateImagePostRequest imagePostRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the user making the request.
            // User user = HttpContext.GetUser();

            // Check if user owns the post.
            bool userOwnsPost = true;

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
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the user making the request.
            // User user = HttpContext.GetUser();

            // Check if user owns the post.
            bool userOwnsPost = true;

            if(!userOwnsPost) 
            {
                return Forbid();
            }

            // Try to delete the image.
            bool success = await _imagePostService.Delete(id);

            if(success)
            {
                return NoContent();
            } 
            else
            {
                return NotFound();
            }
        }
    }
}
