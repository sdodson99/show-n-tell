using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Extensions;
using ShowNTell.API.Models.Responses;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("profiles/{username}")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IFollowService _followService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfilesController> _logger;

        public ProfilesController(IProfileService profileService, IFollowService followService,
            IMapper mapper, ILogger<ProfilesController> logger)
        {
            _profileService = profileService;
            _followService = followService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a user profile.
        /// </summary>
        /// <param name="username">The username of the profile.</param>
        /// <returns>The profile for the username.</returns>
        /// <response code="200">Returns the profile for the username.</response>
        /// <response code="404">Profile does not exist.</response>
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<ProfileResponse>> GetProfile(string username)
        {
            User profile = await _profileService.GetProfile(username);

            if(profile == null) 
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileResponse>(profile));
        }

        /// <summary>
        /// Get a user profile's image posts.
        /// </summary>
        /// <param name="username">The username of the profile.</param>
        /// <returns>The profile's list of image posts.</returns>
        /// <response code="200">Returns the profile's list of image posts.</response>
        [Produces("application/json")]
        [HttpGet("imageposts")]
        public async Task<ActionResult<IEnumerable<ImagePostResponse>>> GetImagePosts(string username)
        {
            IEnumerable<ImagePost> imagePosts = await _profileService.GetImagePosts(username);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(imagePosts));
        }

        /// <summary>
        /// Follow a user's profile.
        /// </summary>
        /// <param name="username">The username of the profile to follow.</param>
        /// <returns>The created follow.</returns>
        /// <response code="200">Returns the created follow.</response>
        /// <response code="400">User attempting to follow themselves.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Profile to follow does not exist.</response>
        [Produces("application/json")]
        [Authorize]
        [HttpPost("follow")]
        public async Task<ActionResult<FollowResponse>> Follow(string username)
        {
            User currentUser = HttpContext.GetUser();

            try
            {
                Follow newFollow = await _followService.FollowUser(username, currentUser.Email);

                return Ok(_mapper.Map<FollowResponse>(newFollow));
            }
            catch (EntityNotFoundException<string>)
            {
                return NotFound();
            }
            catch (OwnProfileFollowException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Unfollow a user's profile.
        /// </summary>
        /// <param name="username">The username of the profile to unfollow.</param>
        /// <response code="204">Successfully unfollowed profile.</response>
        /// <response code="400">Failed to unfollow profile.</response>
        /// <response code="401">Unauthorized.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpDelete("follow")]
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