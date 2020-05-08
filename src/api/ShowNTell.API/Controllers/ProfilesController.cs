using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Authorization;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Services.CurrentUsers;
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
        private readonly ICurrentUserService _currentUserService;

        public ProfilesController(IProfileService profileService, IFollowService followService,
            IMapper mapper, ILogger<ProfilesController> logger, ICurrentUserService currentUserService)
        {
            _profileService = profileService;
            _followService = followService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Get a user profile.
        /// </summary>
        /// <param name="username">The username of the profile.</param>
        /// <returns>The profile for the username.</returns>
        /// <response code="200">Returns the profile for the username.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Profile does not exist.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.READ_ACCESS)]
        [HttpGet]
        public async Task<ActionResult<ProfileResponse>> GetProfile(string username)
        {
            _logger.LogInformation("Received get profile request.");
            _logger.LogInformation("Profile username: {0}", username);
            
            User profile = await _profileService.GetProfile(username);

            if(profile == null) 
            {
                _logger.LogError("Profile with username '{0}' does not exist.", username);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved profile for username {0}.", profile.Username);

            return Ok(_mapper.Map<ProfileResponse>(profile));
        }

        /// <summary>
        /// Get a user profile's image posts.
        /// </summary>
        /// <param name="username">The username of the profile.</param>
        /// <returns>The profile's list of image posts.</returns>
        /// <response code="200">Returns the profile's list of image posts.</response>
        /// <response code="403">Forbidden.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.READ_ACCESS)]
        [HttpGet("imageposts")]
        public async Task<ActionResult<IEnumerable<ImagePostResponse>>> GetImagePosts(string username)
        {
            _logger.LogInformation("Received get profile image posts request.");
            _logger.LogInformation("Profile username: {0}", username);
            
            IEnumerable<ImagePost> imagePosts = await _profileService.GetImagePosts(username);

            _logger.LogInformation("Successfully retrieved profile image posts for username {0}.", username);

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
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Profile to follow does not exist.</response>
        [Produces("application/json")]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPost("follow")]
        public async Task<ActionResult<FollowingResponse>> Follow(string username)
        {
            _logger.LogInformation("Received profile follow request.");
            _logger.LogInformation("Profile username: {0}", username);
            
            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            try
            {
                Follow newFollow = await _followService.FollowUser(username, currentUser.Email);
                _logger.LogInformation("Successfully followed '{0}' for '{1}'.", username, currentUser.Email);

                return Ok(_mapper.Map<FollowingResponse>(newFollow));
            }
            catch (EntityNotFoundException<string>)
            {
                _logger.LogError("Profile with username '{0}' does not exist.", username);
                return NotFound();
            }
            catch (OwnProfileFollowException)
            {
                _logger.LogError("User '{0}' cannot follow their own profile '{1}'.", currentUser.Email, username);
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
        /// <response code="403">Forbidden.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpDelete("follow")]
        public async Task<IActionResult> Unfollow(string username)
        {
            _logger.LogInformation("Received profile unfollow request.");
            _logger.LogInformation("Profile username: {0}", username);
            
            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            bool success = await _followService.UnfollowUser(username, currentUser.Email);

            if(!success)
            {
                _logger.LogError("Failed to unfollow '{0}' for '{1}'.", username, currentUser.Email);
                return BadRequest();
            }

            _logger.LogInformation("Successfully unfollowed '{0}' for '{1}'.", username, currentUser.Email);

            return NoContent();
        }
    }
}