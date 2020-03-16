using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.API.Models.Responses;
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
        private readonly IMapper _mapper;

        public ProfilesController(IImagePostService imagePostService, IMapper mapper)
        {
            _imagePostService = imagePostService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("imageposts")]
        public async Task<IActionResult> GetImagePosts(string username)
        {
            IEnumerable<ImagePost> imagePosts = await _imagePostService.GetAllByUsername(username);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(imagePosts));
        }
    }
}