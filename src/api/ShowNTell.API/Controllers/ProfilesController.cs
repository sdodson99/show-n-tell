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
        private readonly IProfileService _profileService;
        private readonly IFollowService _followService;
        private readonly IMapper _mapper;

        public ProfilesController(IProfileService profileService, IFollowService followService, IMapper mapper)
        {
            _profileService = profileService;
            _followService = followService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(string username)
        {
            User profile = await _profileService.GetProfile(username);

            return Ok(_mapper.Map<ProfileResponse>(profile));
        }

        [HttpGet]
        [Route("imageposts")]
        public async Task<IActionResult> GetImagePosts(string username)
        {
            IEnumerable<ImagePost> imagePosts = await _profileService.GetImagePosts(username);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(imagePosts));
        }
    }
}