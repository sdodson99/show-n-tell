using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTest : ControllerTest<AuthenticationController>
    {
        private Mock<IUserService> _mockUserService;
        private AuthenticationController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new AuthenticationController(_mockUserService.Object, _mapper, _logger, _currentUserService);
        }

        [Test]
        public async Task GoogleLogin_WithExistingUser_ReturnsOk()
        {
            _mockUserService.Setup(s => s.GetByEmail(It.IsAny<string>())).ReturnsAsync(CurrentUser);
            Type expectedType = typeof(OkObjectResult);

            ActionResult<UserResponse> actual = await _controller.GoogleLogin();
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GoogleLogin_WithNonExistingUser_ReturnsOk()
        {
            _mockUserService.Setup(s => s.Create(It.IsAny<User>())).ReturnsAsync(CurrentUser);
            Type expectedType = typeof(OkObjectResult);

            ActionResult<UserResponse> actual = await _controller.GoogleLogin();
            ActionResult actualResult = actual.Result;

            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GoogleLogin_WithNonExistingUser_CreatesNewUser()
        {
            _mockUserService.Setup(s => s.Create(It.IsAny<User>())).ReturnsAsync(CurrentUser).Verifiable();

            ActionResult<UserResponse> actual = await _controller.GoogleLogin();

            _mockUserService.Verify(s => s.Create(It.IsAny<User>()), Times.Once);
        }
    }
}