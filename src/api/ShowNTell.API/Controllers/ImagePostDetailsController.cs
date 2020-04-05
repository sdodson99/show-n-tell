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
using Microsoft.AspNetCore.Http;

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

        public ImagePostDetailsController(ILikeService likeService, ICommentService commentService, 
            IMapper mapper, ILogger<ImagePostDetailsController> logger)
        {
            _likeService = likeService;
            _commentService = commentService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a comment on an image post.
        /// </summary>
        /// <param name="id">The id of the image post.</param>
        /// <param name="commentRequest">The new comment to create.</param>
        /// <returns>The created comment.</returns>
        /// <response code="200">Returns the created comment.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Failed to create comment.</response>
        [Authorize]
        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponse>> CreateComment(int id, [FromBody] CreateCommentRequest commentRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            User currentUser = HttpContext.GetUser();
            
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

        /// <summary>
        /// Like an image post.
        /// </summary>
        /// <param name="id">The id of the image post.</param>
        /// <returns>The created like.</returns>
        /// <response code="200">Returns the created like.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">Failed to create like.</response>
        [Authorize]
        [HttpPost("like")]
        public async Task<ActionResult<LikeResponse>> LikeImagePost(int id)
        {
            User currentUser = HttpContext.GetUser();
            
            try
            {
                Like createdLike = await _likeService.LikeImagePost(id, currentUser.Email);
                
                return Ok(_mapper.Map<LikeResponse>(createdLike));
            }
            catch (DuplicateLikeException)
            {
                return BadRequest();
            }
            catch (OwnImagePostLikeException)
            {
                return BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Unlike an image post.
        /// </summary>
        /// <param name="id">The id of the image post to unlike.</param>
        /// <response code="204">Successfully unliked image post.</response>
        /// <response code="400">Failed to unlike image post.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpDelete("like")]
        public async Task<IActionResult> UnlikeImagePost(int id)
        {
            User currentUser = HttpContext.GetUser();

            if(!await _likeService.UnlikeImagePost(id, currentUser.Email))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}