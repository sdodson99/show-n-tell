using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class ProfilesControllerTest : ControllerTests<ProfilesController>
    {
        private Mock<IProfileService> _mockProfileService;
        private Mock<IFollowService> _mockFollowService;
        private ProfilesController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockProfileService = new Mock<IProfileService>();
            _mockFollowService = new Mock<IFollowService>();

            _controller = new ProfilesController(_mockProfileService.Object, _mockFollowService.Object, 
                _mapper, _logger, _currentUserService);
        }

        [Test]
        public async Task GetProfile_WithExistingProfileUsername_ReturnsOk()
        {
            _mockProfileService.Setup(s => s.GetProfile(It.IsAny<string>())).ReturnsAsync(new User());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<ProfileResponse> actual = await _controller.GetProfile(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetProfile_WithExistingProfileUsername_ReturnsNotFound()
        {
            Type expectedType = typeof(NotFoundResult);

            ActionResult<ProfileResponse> actual = await _controller.GetProfile(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetImagePosts_ReturnsOk()
        {
            _mockProfileService.Setup(s => s.GetImagePosts(It.IsAny<string>())).ReturnsAsync(new List<ImagePost>());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<IEnumerable<ImagePostResponse>> actual = await _controller.GetImagePosts(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Follow_WithExistingProfileUsername_ReturnsOk()
        {
            _mockFollowService.Setup(s => s.FollowUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Follow());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<FollowingResponse> actual = await _controller.Follow(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Follow_WithNonExistingProfileUsername_ReturnsNotFound()
        {
            _mockFollowService.Setup(s => s.FollowUser(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new EntityNotFoundException<string>(It.IsAny<string>()));
            Type expectedType = typeof(NotFoundResult);

            ActionResult<FollowingResponse> actual = await _controller.Follow(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Follow_WithUserFollowingOwnProfile_ReturnsBadRequest()
        {
            _mockFollowService.Setup(s => s.FollowUser(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new OwnProfileFollowException(It.IsAny<string>()));
            Type expectedType = typeof(BadRequestResult);

            ActionResult<FollowingResponse> actual = await _controller.Follow(It.IsAny<string>());
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Unfollow_WithSuccessfulUnfollow_ReturnsNoContent()
        {
            _mockFollowService.Setup(s => s.UnfollowUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _controller.Unfollow(It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Unfollow_WithUnsuccessfulUnfollow_ReturnsBadRequest()
        {
            _mockFollowService.Setup(s => s.UnfollowUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.Unfollow(It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }
    }
}