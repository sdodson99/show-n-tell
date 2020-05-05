using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Models.Requests;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class ImagePostDetailsControllerTest : ControllerTests<ImagePostDetailsController>
    {
        private Mock<ILikeService> _mockLikeService;
        private Mock<ICommentService> _mockCommentService;
        private ImagePostDetailsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockLikeService = new Mock<ILikeService>();
            _mockCommentService = new Mock<ICommentService>();

            _controller = new ImagePostDetailsController(_mockLikeService.Object, _mockCommentService.Object, 
                _mapper, _logger, _currentUserService);
        }

        [Test]
        public async Task CreateComment_WithValidComment_ReturnsOk()
        {
            _mockCommentService.Setup(s => s.Create(It.IsAny<Comment>())).ReturnsAsync(new Comment());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<CommentResponse> actual = await _controller.CreateComment(1, new CreateCommentRequest());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task CreateComment_WithInvalidComment_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("key", "error message");
            Type expectedType = typeof(BadRequestResult);

            ActionResult<CommentResponse> actual = await _controller.CreateComment(It.IsAny<int>(), new CreateCommentRequest());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task UpdateComment_WithValidUpdateRequest_ReturnsOk()
        {
            _mockCommentService.Setup(s => s.IsCommentOwner(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockCommentService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new Comment());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<CommentResponse> actual = await _controller.UpdateComment(It.IsAny<int>(), new UpdateCommentRequest());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult); 
        }

        [Test]
        public async Task UpdateComment_WithCommentNotOwnedByUser_ReturnsForbid()
        {
            _mockCommentService.Setup(s => s.IsCommentOwner(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(ForbidResult);

            ActionResult<CommentResponse> actual = await _controller.UpdateComment(It.IsAny<int>(), new UpdateCommentRequest());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult); 
        }

        [Test]
        public async Task UpdateComment_WithNonExistingComment_ReturnsNotFound()
        {
            _mockCommentService.Setup(s => s.IsCommentOwner(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockCommentService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException());
            Type expectedType = typeof(NotFoundResult);

            ActionResult<CommentResponse> actual = await _controller.UpdateComment(It.IsAny<int>(), new UpdateCommentRequest());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult); 
        }

        [Test]
        public async Task DeleteComment_WithValidCommentDelete_ReturnsNoContent()
        {
            _mockCommentService.Setup(s => s.CanDelete(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockCommentService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _controller.DeleteComment(It.IsAny<int>());

            Assert.IsAssignableFrom(expectedType, actual); 
        }

        [Test]
        public async Task DeleteComment_WithUnsuccessfulCommentDelete_ReturnsNotFound()
        {
            _mockCommentService.Setup(s => s.CanDelete(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockCommentService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(false);
            Type expectedType = typeof(NotFoundResult);

            IActionResult actual = await _controller.DeleteComment(It.IsAny<int>());

            Assert.IsAssignableFrom(expectedType, actual); 
        }

        [Test]
        public async Task DeleteComment_WithCommentOrImagePostNotOwnedByUser_ReturnsForbid()
        {
            _mockCommentService.Setup(s => s.CanDelete(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(ForbidResult);

            IActionResult actual = await _controller.DeleteComment(It.IsAny<int>());

            Assert.IsAssignableFrom(expectedType, actual); 
        }

        [Test]
        public async Task LikeImagePost_WithValidLike_ReturnsOk()
        {
            _mockLikeService.Setup(s => s.LikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new Like());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<LikeResponse> actual = await _controller.LikeImagePost(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task LikeImagePost_WithUserDuplicatingLike_ReturnsBadRequest()
        {
            _mockLikeService.Setup(s => s.LikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new DuplicateLikeException(It.IsAny<ImagePost>(), It.IsAny<string>()));
            Type expectedType = typeof(BadRequestResult);

            ActionResult<LikeResponse> actual = await _controller.LikeImagePost(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task LikeImagePost_WithUserLikingOwnImagePost_ReturnsBadRequest()
        {
            _mockLikeService.Setup(s => s.LikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new OwnImagePostLikeException(It.IsAny<ImagePost>(), It.IsAny<string>()));
            Type expectedType = typeof(BadRequestResult);

            ActionResult<LikeResponse> actual = await _controller.LikeImagePost(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task LikeImagePost_WithNonExistingImagePost_ReturnsBadRequest()
        {
            _mockLikeService.Setup(s => s.LikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException<int>(It.IsAny<int>()));
            Type expectedType = typeof(BadRequestResult);

            ActionResult<LikeResponse> actual = await _controller.LikeImagePost(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task LikeImagePost_WithNonExistingUser_ReturnsBadRequest()
        {
            _mockLikeService.Setup(s => s.LikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException<string>(It.IsAny<string>()));
            Type expectedType = typeof(BadRequestResult);

            ActionResult<LikeResponse> actual = await _controller.LikeImagePost(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task UnlikeImagePost_WithSuccessfulUnlike_ReturnsNoContent()
        {
            _mockLikeService.Setup(s => s.UnlikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _controller.UnlikeImagePost(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task UnlikeImagePost_WithUnsuccessfulUnlike_ReturnsBadRequest()
        {
            _mockLikeService.Setup(s => s.UnlikeImagePost(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.UnlikeImagePost(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }
    }
}