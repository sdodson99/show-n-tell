using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Exceptions;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Models.Requests;
using Microsoft.AspNetCore.Http;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.API.Authorization;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("imageposts/{imagePostId:int}")]
    public class ImagePostDetailsController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly ILogger<ImagePostDetailsController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ImagePostDetailsController(ILikeService likeService, ICommentService commentService,
            IMapper mapper, ILogger<ImagePostDetailsController> logger, ICurrentUserService currentUserService)
        {
            _likeService = likeService;
            _commentService = commentService;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Create a comment on an image post.
        /// </summary>
        /// <param name="imagePostId">The id of the image post.</param>
        /// <param name="commentRequest">The new comment to create.</param>
        /// <returns>The created comment.</returns>
        /// <response code="200">Returns the created comment.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Failed to create comment.</response>
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponse>> CreateComment(int imagePostId, [FromBody] CreateCommentRequest commentRequest)
        {
            _logger.LogInformation("Received image post comment create request.");
            _logger.LogInformation("Image post id: {0}", imagePostId);

            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid create comment request model state.");
                return BadRequest();
            }

            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);
            
            Comment createdComment = new Comment()
            {
                ImagePostId = imagePostId,
                UserEmail = currentUser.Email,
                DateCreated = DateTime.Now,
                Content = commentRequest.Content   
            };

            createdComment = await _commentService.Create(createdComment);

            _logger.LogInformation("Successfully created comment with id {0} on image post with id {1}.", createdComment.Id, createdComment.ImagePostId);

            return Ok(_mapper.Map<CommentResponse>(createdComment));
        }

        /// <summary>
        /// Update a comment.
        /// </summary>
        /// <param name="commentId">The id of the comment to update.</param>
        /// <param name="updateCommentRequest">The details of the updated comment.</param>
        /// <returns>The updated comment.</returns>
        /// <response code="200">Returns the updated comment.</response>
        /// <response code="400">Update comment response invalid.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Comment to update not found.</response>      
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPut("comments/{commentId:int}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(int commentId, [FromBody] UpdateCommentRequest updateCommentRequest)
        {
            _logger.LogInformation("Received comment update request.");
            _logger.LogInformation("Comment id: {0}", commentId);

            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            if(!await _commentService.IsCommentOwner(commentId, currentUser.Email))
            {
                _logger.LogError("User '{0}' does not own comment with id {1}.", currentUser.Email, commentId);
                return Forbid();
            }

            try
            {
                _logger.LogInformation("Updating content of comment with id {0} to '{1}'.", commentId, updateCommentRequest.Content);
                Comment updatedComment = await _commentService.Update(commentId, updateCommentRequest.Content);
                _logger.LogInformation("Successfully updated comment with id {0}.", commentId);

                return Ok(_mapper.Map<CommentResponse>(updatedComment));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogError("Comment with id {0} does not exist.", commentId);
                return NotFound();
            }
        } 

        /// <summary>
        /// Delete a comment.
        /// </summary>
        /// <param name="commentId">The id of the comment to delete.</param>
        /// <response code="204">Returns the created comment.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Comment to delete not found.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpDelete("comments/{commentId:int}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            _logger.LogInformation("Received comment delete request.");
            _logger.LogInformation("Comment id: {0}", commentId);

            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            if(!await _commentService.CanDelete(commentId, currentUser.Email))
            {
                _logger.LogError("User '{0}' does not have permission to delete comment with id {1}.", currentUser.Email, commentId);
                return Forbid();
            }

            if(!await _commentService.Delete(commentId))
            {
                _logger.LogError("Failed to delete comment with id {0}.", commentId);
                return NotFound();
            }

            _logger.LogError("Successfully deleted comment with id {0}.", commentId);
            return NoContent();
        } 

        /// <summary>
        /// Like an image post.
        /// </summary>
        /// <param name="imagePostId">The id of the image post.</param>
        /// <returns>The created like.</returns>
        /// <response code="200">Returns the created like.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Failed to create like.</response>
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpPost("like")]
        public async Task<ActionResult<LikeResponse>> LikeImagePost(int imagePostId)
        {
            _logger.LogInformation("Received image post like request.");
            _logger.LogInformation("Image post id: {0}", imagePostId);

            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);
            
            try
            {
                Like createdLike = await _likeService.LikeImagePost(imagePostId, currentUser.Email);
                
                _logger.LogInformation("Successfully created like by '{0}' on image post with id {1}.", createdLike.UserEmail, createdLike.ImagePostId);

                return Ok(_mapper.Map<LikeResponse>(createdLike));
            }
            catch (DuplicateLikeException)
            {
                _logger.LogError("User '{0}' cannot like an image post with id {1} more than once.", currentUser.Email, imagePostId);
                return BadRequest();
            }
            catch (OwnImagePostLikeException)
            {
                _logger.LogError("User '{0}' cannot like their own image post with id {1}.", currentUser.Email, imagePostId);
                return BadRequest();
            }
            catch (EntityNotFoundException<int>)
            {
                _logger.LogError("Image post with id {0} does not exist.", imagePostId);
                return BadRequest();
            }
            catch (EntityNotFoundException<string>)
            {
                _logger.LogError("User with email '{0}' does not exist.", currentUser.Email);
                return BadRequest();
            }
        }

        /// <summary>
        /// Unlike an image post.
        /// </summary>
        /// <param name="imagePostId">The id of the image post to unlike.</param>
        /// <response code="204">Successfully unliked image post.</response>
        /// <response code="400">Failed to unlike image post.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = PolicyName.REQUIRE_AUTH_WRITE_ACCESS)]
        [HttpDelete("like")]
        public async Task<IActionResult> UnlikeImagePost(int imagePostId)
        {
            _logger.LogInformation("Received image post unlike request.");
            _logger.LogInformation("Image post id: {0}", imagePostId);
            
            User currentUser = _currentUserService.GetCurrentUser(HttpContext);
            _logger.LogInformation("Requesting user email: {0}", currentUser.Email);

            if(!await _likeService.UnlikeImagePost(imagePostId, currentUser.Email))
            {
                _logger.LogError("Failed to delete like by '{0}' on image post with id '{1}'.", currentUser.Email, imagePostId);
                return BadRequest();
            }

            _logger.LogInformation("Successfully deleted like by '{0}' on image post with id {1}.", currentUser.Email, imagePostId);
            return NoContent();
        }
    }
}