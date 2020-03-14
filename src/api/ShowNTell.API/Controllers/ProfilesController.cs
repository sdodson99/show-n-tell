using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("[controller]/{username}")]
    public class ProfilesController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;

        public ProfilesController(IImagePostService imagePostService)
        {
            _imagePostService = imagePostService;
        }

        [HttpGet]
        [Route("imageposts")]
        public async Task<IActionResult> GetImagePosts(string username)
        {
            IEnumerable<ImagePost> imagePosts = await _imagePostService.GetAllByUsername(username);

            return Ok(imagePosts);
        }
    }
}