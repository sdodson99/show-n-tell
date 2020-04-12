using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IFeedService _feedService;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public FeedController(IFeedService feedService,
            IMapper mapper, ILogger<FeedController> logger, ICurrentUserService currentUserService)
        {
            _feedService = feedService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Get a feed of image posts.
        /// </summary>
        /// <returns>The feed of image posts.</returns>
        /// <response code="200">Returns the feed of image posts.</response>
        /// <response code="401">Unauthorized.</response>
        [Produces("application/json")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagePostResponse>>> GetFeed()
        {
            _logger.LogInformation("Received feed request.");
            
            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            IEnumerable<ImagePost> feed = await _feedService.GetFeed(currentUser.Email);
            
            _logger.LogInformation("Successfully retrieved feed of {0} image posts.", feed.Count());

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(feed));
        }
    }
}