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
    public class ImagePostController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly IImageSaver _imageSaver;
        private readonly ILogger<ImagePostController> _logger;

        public ImagePostController(IImagePostService imagePostService, IImageSaver imageSaver, ILogger<ImagePostController> logger)
        {
            _imagePostService = imagePostService;
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
            return Ok(new ImagePost()
            {
                Description = "hello world",
                ImageUri = "https://images.pexels.com/photos/255379/pexels-photo-255379.jpeg",
                DateCreated = DateTime.Now,
                Id = 5
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _imagePostService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateImagePostRequest imagePostRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Store image file.
            IFormFile image = imagePostRequest.Image;
            string imageUri = await _imageSaver.SaveImage(image.OpenReadStream(), Path.GetExtension(image.FileName));

            // Save image database record.
            ImagePost newImagePost = new ImagePost()
            {
                Description = imagePostRequest.Description,
                ImageUri = imageUri,
                DateCreated = DateTime.Now
            };

            newImagePost = await _imagePostService.Create(newImagePost);

            return Ok(newImagePost);
        }
    }
}
