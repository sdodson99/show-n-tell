using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using System;
using ShowNTell.API.Models.Responses;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Services.CurrentUsers;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserService userService,
            IMapper mapper, ILogger<AuthenticationController> logger, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }


        /// <summary>
        /// Login to Show 'N Tell with a Google account. Authenticate with a Google token to make this request.
        /// </summary>
        /// <returns>The logged in user.</returns>
        /// <response code="200">Returns the logged in user.</response>
        /// <response code="401">Unauthorized.</response>
        [Produces("application/json")]
        [Authorize]
        [HttpPost("google")]
        public async Task<ActionResult<UserResponse>> GoogleLogin()
        {
            _logger.LogInformation("Received Google login request.");

            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            User existingUser = await _userService.GetByEmail(currentUser.Email);
            
            // User does not exist.
            if(existingUser == null)
            {
                _logger.LogWarning("User '{0}' does not exist.", currentUser.Email);
                _logger.LogInformation("Creating account for '{0}'.", currentUser.Email);
                currentUser.DateJoined = DateTime.Now;
                existingUser = await _userService.Create(currentUser);
            }

            _logger.LogInformation("Successfully logged in user '{0}'.", existingUser.Email);

            return Ok(_mapper.Map<UserResponse>(existingUser));
        }
    }
}