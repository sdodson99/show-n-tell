using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.Domain.Services;

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
        

    }
}
