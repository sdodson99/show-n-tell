using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowNTell.API.Extensions;
using ShowNTell.API.Models.Responses;
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

        public FeedController(IFeedService feedService, 
            IMapper mapper, ILogger<FeedController> logger)
        {
            _feedService = feedService;
            _mapper = mapper;
            _logger = logger;
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
            User currentUser = HttpContext.GetUser();

            IEnumerable<ImagePost> feed = await _feedService.GetFeed(currentUser.Email);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(feed));
        }
    }
}