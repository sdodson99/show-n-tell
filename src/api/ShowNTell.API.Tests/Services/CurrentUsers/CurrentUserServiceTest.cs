using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Tests.Services.CurrentUsers
{
    [TestFixture]
    public class CurrentUserServiceTest
    {
        private CurrentUserService _currentUserService;

        [SetUp]
        public void SetUp()
        {
            _currentUserService = new CurrentUserService();
        }

        [Test]
        public void GetCurrentUser_WithHttpContextContainingUser_ReturnsUserWithEmailAndUsername()
        {
            string expectedEmail = "test@gmail.com";
            string expectedUsername = "test";
            HttpContext context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, expectedEmail)
                    })
                })
            };

            User actual = _currentUserService.GetCurrentUser(context);
            string actualEmail = actual.Email;
            string actualUsername = actual.Username;

            Assert.AreEqual(expectedEmail, actualEmail);
            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public void GetCurrentUser_WithHttpContextContainingNoEmailClaim_ReturnsNull()
        {
            HttpContext context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>())
                })
            };

            User actual = _currentUserService.GetCurrentUser(context);

            Assert.IsNull(actual);
        }

        [Test]
        public void GetCurrentUser_WithClaimsContainingUser_ReturnsUserWithEmailAndUsername()
        {
            string expectedEmail = "test@gmail.com";
            string expectedUsername = "test";
            ClaimsPrincipal claims = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, expectedEmail)
                })
            });

            User actual = _currentUserService.GetCurrentUser(claims);
            string actualEmail = actual.Email;
            string actualUsername = actual.Username;

            Assert.AreEqual(expectedEmail, actualEmail);
            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public void GetCurrentUser_WithClaimsContainingNoEmailClaim_ReturnsNull()
        {
            ClaimsPrincipal claims = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>())
            });

            User actual = _currentUserService.GetCurrentUser(claims);

            Assert.IsNull(actual);
        }
    }
}