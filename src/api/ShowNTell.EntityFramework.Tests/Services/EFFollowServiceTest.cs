using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFFollowServiceTest
    {
        private const string EXISTING_USER_EMAIL_1 = "test1@gmail.com";
        private const string EXISTING_USER_EMAIL_2 = "test2@gmail.com";
        private const string EXISTING_FOLLOW_USER_EMAIL = "followed@gmail.com";
        private const string EXISTING_FOLLOW_FOLLOWER_EMAIL = "follower@gmail.com";

        private string _databaseName;

        private EFFollowService _followService;

        [SetUp]
        public void Setup()
        {
            _databaseName = Guid.NewGuid().ToString();

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(() => GetDbContext());

            _followService = new EFFollowService(contextFactory.Object);
        }

        [Test]
        public async Task FollowUser_WithExistingUsers_ReturnsSuccessfulFollow()
        {
            string expectedUserEmail = EXISTING_USER_EMAIL_1;
            string expectedFollowerEmail = EXISTING_USER_EMAIL_2;

            Follow actualFollow = await _followService.FollowUser(expectedUserEmail, expectedFollowerEmail);
            string actualUserEmail = actualFollow.UserEmail;
            string actualFollowerEmail = actualFollow.FollowerEmail;

            Assert.AreEqual(expectedUserEmail, actualUserEmail);
            Assert.AreEqual(expectedFollowerEmail, actualFollowerEmail);
        }

        [Test]
        public void FollowUser_WithExistingFollow_ThrowsInvalidOperationException()
        {
            string expectedUserEmail = EXISTING_FOLLOW_USER_EMAIL;
            string expectedFollowerEmail = EXISTING_FOLLOW_FOLLOWER_EMAIL;

            InvalidOperationException actualException = Assert.ThrowsAsync<InvalidOperationException>(() => _followService.FollowUser(expectedUserEmail, expectedFollowerEmail));
        }

        [Test]
        public async Task UnfollowUser_WithExistingFollow_ReturnsTrue()
        {
            string expectedUserEmail = EXISTING_FOLLOW_USER_EMAIL;
            string expectedFollowerEmail = EXISTING_FOLLOW_FOLLOWER_EMAIL;

            bool actual = await _followService.UnfollowUser(expectedUserEmail, expectedFollowerEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task UnfollowUser_WithNonExistingFollow_ReturnsFalse()
        {
            string expectedUserEmail = EXISTING_USER_EMAIL_1;
            string expectedFollowerEmail = EXISTING_USER_EMAIL_2;

            bool actual = await _followService.UnfollowUser(expectedUserEmail, expectedFollowerEmail);

            Assert.IsFalse(actual);
        }

        private ShowNTellDbContext GetDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(_databaseName).Options;

            ShowNTellDbContext context = new ShowNTellDbContext(options);

            if(!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Email = EXISTING_USER_EMAIL_1
                });

                context.Users.Add(new User
                {
                    Email = EXISTING_USER_EMAIL_2
                });

                context.Users.Add(new User
                {
                    Email = EXISTING_FOLLOW_USER_EMAIL
                });

                context.Users.Add(new User
                {
                    Email = EXISTING_FOLLOW_FOLLOWER_EMAIL
                });

                context.Follows.Add(new Follow()
                {
                    UserEmail = EXISTING_FOLLOW_USER_EMAIL,
                    FollowerEmail = EXISTING_FOLLOW_FOLLOWER_EMAIL
                });
            }

            context.SaveChanges();

            return context;
        }
    }
}
