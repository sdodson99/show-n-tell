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
using ShowNTell.API.Models.Requests;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("imageposts/{id:int}")]
    public class ImagePostDetailsController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly ILogger<ImagePostDetailsController> _logger;

        public ImagePostDetailsController(ILikeService likeService, ICommentService commentService, IMapper mapper, ILogger<ImagePostDetailsController> logger)
        {
            _likeService = likeService;
            _commentService = commentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("comments")]
        [Authorize]
        public async Task<IActionResult> CreateComment(int id, [FromBody] CreateCommentRequest commentRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            User currentUser = HttpContext.GetUser();

            try
            {
                Comment createdComment = new Comment()
                {
                    ImagePostId = id,
                    UserEmail = currentUser.Email,
                    DateCreated = DateTime.Now,
                    Content = commentRequest.Content   
                };

                createdComment = await _commentService.Create(createdComment);

                return Ok(_mapper.Map<CommentResponse>(createdComment));
            }
            catch (Exception)
            {
                return BadRequest();
            }
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