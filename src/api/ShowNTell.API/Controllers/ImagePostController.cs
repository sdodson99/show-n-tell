using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Models.Requests;
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
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ImagePostRequest imagePostRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IFormFile image = imagePostRequest.Image;
            Stream stream = image.OpenReadStream();

            string imageUri = await _imageSaver.SaveImage(stream);

            return Ok();
        }
    }
}
