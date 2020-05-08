using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using ShowNTell.EntityFramework.Tests.BaseFixtures;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFUserServiceTest : EFTest
    {
        private EFUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new EFUserService(_contextFactory);
        }

        [Test]
        public async Task GetByEmail_WithExistingEmail_ReturnsUserWithEmail()
        {
            string expectedEmail = "test@gmail.com";
            ShowNTellDbContext context = _contextFactory.CreateDbContext();
            context.Add(new User(){ Email = expectedEmail });
            context.SaveChanges();

            User user = await _userService.GetByEmail(expectedEmail);
            string actualEmail = user.Email;

            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public async Task GetByEmail_WithUserFollowingOthers_ReturnsUserWithFollows()
        {
            int expectedFollowingCount = 3;
            ShowNTellDbContext context = _contextFactory.CreateDbContext();
            context.Add(new User(){ Email = string.Empty, Following = new List<Follow>() 
            {
                new Follow() { User = new User() { Email = "user1@gmail.com" }},
                new Follow() { User = new User() { Email = "user2@gmail.com" }},
                new Follow() { User = new User() { Email = "user3@gmail.com" }}
            }});
            context.SaveChanges();

            User user = await _userService.GetByEmail(string.Empty);
            int actualFollowingCount = user.Following.Count;

            Assert.IsNotNull(user.Following);
            Assert.AreEqual(expectedFollowingCount, actualFollowingCount);
        }

        [Test]
        public async Task GetByEmail_WithNonExistingEmail_ReturnsNull()
        {
            User user = await _userService.GetByEmail(It.IsAny<string>());

            Assert.IsNull(user);
        }

        [Test]
        public async Task Create_WithNonExistingEmail_ReturnsNewUserWithEmail()
        {
            string expectedEmail = "test@gmail.com";
            User newUser = new User() { Email = expectedEmail };

            User createdUser = await _userService.Create(newUser);
            string actualEmail = createdUser.Email;

            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void Create_WithExistingEmail_ThrowsArgumentExceptionForEmail()
        {
            string expectedInvalidParamName = "email";
            string existingEmail = "test@gmail.com";
            ShowNTellDbContext context = _contextFactory.CreateDbContext();
            context.Add(new User() { Email = existingEmail });
            context.SaveChanges();
            User newUser = new User() { Email = existingEmail };

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _userService.Create(newUser));
            string actualInvalidParamName = exception.ParamName;

            Assert.AreEqual(expectedInvalidParamName, actualInvalidParamName);
        }
    }
}