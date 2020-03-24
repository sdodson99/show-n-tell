using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.API.Extensions;
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

            if(profile == null) 
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileResponse>(profile));
        }

        [HttpGet]
        [Route("imageposts")]
        public async Task<IActionResult> GetImagePosts(string username)
        {
            IEnumerable<ImagePost> imagePosts = await _profileService.GetImagePosts(username);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(imagePosts));
        }

        [HttpPost]
        [Route("follow")]
        [Authorize]
        public async Task<IActionResult> Follow(string username)
        {
            User currentUser = HttpContext.GetUser();

            try
            {
                Follow newFollow = await _followService.FollowUser(username, currentUser.Email);

                return Ok(_mapper.Map<FollowResponse>(newFollow));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("follow")]
        [Authorize]
        public async Task<IActionResult> Unfollow(string username)
        {
            User currentUser = HttpContext.GetUser();

            bool success = await _followService.UnfollowUser(username, currentUser.Email);

            if(!success)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}