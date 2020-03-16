using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.API.Extensions;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Exceptions;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("imageposts/{id:int}")]
    public class ImagePostDetailsController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;
        private readonly ILogger<ImagePostDetailsController> _logger;

        public ImagePostDetailsController(ILikeService likeService, IMapper mapper, ILogger<ImagePostDetailsController> logger)
        {
            _likeService = likeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("like")]
        [Authorize]
        public async Task<IActionResult> LikeImagePost(int id)
        {
            User currentUser = HttpContext.GetUser();
            
            try
            {
                Like createdLike = await _likeService.LikeImagePost(id, currentUser.Email);
                
                return Ok(_mapper.Map<LikeResponse>(createdLike));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("like")]
        [Authorize]
        public async Task<IActionResult> UnlikeImagePost(int id)
        {
            User currentUser = HttpContext.GetUser();

            await _likeService.UnlikeImagePost(id, currentUser.Email);

            return NoContent();
        }
    }
}