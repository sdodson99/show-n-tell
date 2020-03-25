using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.API.Extensions;
using ShowNTell.API.Models.Responses;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly IFeedService _feedService;
        private readonly IMapper _mapper;

        public FeedController(IFeedService feedService, IMapper mapper)
        {
            _feedService = feedService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFeed()
        {
            User currentUser = HttpContext.GetUser();

            IEnumerable<ImagePost> feed = await _feedService.GetFeed(currentUser.Email);

            return Ok(_mapper.Map<IEnumerable<ImagePostResponse>>(feed));
        }
    }
}