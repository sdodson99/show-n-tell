using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Authorization.Requirements.AdminOverride;
using ShowNTell.API.Authorization.Requirements.ReadAccess;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Tests.Authorization.Requirements.AdminOverride
{
    [TestFixture]
    public class UsernameAdminOverrideHandlerTests
    {
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private List<string> _adminUsernames;
        private AuthorizationHandlerContext _context;
        private UsernameAdminOverrideHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _adminUsernames = new List<string>();
            _context = new AuthorizationHandlerContext(new []{ new Mock<IAuthorizationRequirement>().Object }, null, null);

            _handler = new UsernameAdminOverrideHandler(_mockCurrentUserService.Object, _adminUsernames);
        }

        [Test]
        public async Task HandleAsync_WithAdminUser_Succeeds()
        {
            string adminUsername = "testuser";
            _adminUsernames.Add(adminUsername);
            _mockCurrentUserService.Setup(s => s.GetCurrentUser(It.IsAny<ClaimsPrincipal>())).Returns(new User(){ Username = adminUsername });

            await _handler.HandleAsync(_context);

            Assert.IsTrue(_context.HasSucceeded);
        }

        [Test]
        public async Task HandleAsync_WithNonAdminUser_DoesNotSucceed()
        {
            _mockCurrentUserService.Setup(s => s.GetCurrentUser(It.IsAny<ClaimsPrincipal>())).Returns(new User());

            await _handler.HandleAsync(_context);

            Assert.IsFalse(_context.HasSucceeded);
        }

        [Test]
        public async Task HandleAsync_WithInvalidUser_DoesNotSucceed()
        {
            await _handler.HandleAsync(_context);

            Assert.IsFalse(_context.HasSucceeded);
        }
    }
}