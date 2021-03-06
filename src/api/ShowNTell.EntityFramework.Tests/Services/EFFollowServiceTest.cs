﻿using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using ShowNTell.EntityFramework.Tests.BaseFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFFollowServiceTest : EFTest
    {
        private const string EXISTING_USER_EMAIL_1 = "test1@gmail.com";
        private const string EXISTING_USER_USERNAME_1 = "test1";
        private const string EXISTING_USER_EMAIL_2 = "test2@gmail.com";
        private const string EXISTING_USER_USERNAME_2 = "test2";
        private const string EXISTING_FOLLOW_USER_EMAIL = "followed@gmail.com";
        private const string EXISTING_FOLLOW_USER_USERNAME = "followed";
        private const string EXISTING_FOLLOW_FOLLOWER_EMAIL = "follower@gmail.com";
        private const string EXISTING_FOLLOW_FOLLOWER_USERNAME = "follower";
        private const string NON_EXISTING_USER_USERNAME = "non_existing_username";

        private EFFollowService _followService;

        [SetUp]
        public void Setup()
        {
            _followService = new EFFollowService(_contextFactory);
        }

        [Test]
        public async Task FollowUser_WithExistingUsers_ReturnsSuccessfulFollow()
        {
            string expectedUserUsername = EXISTING_USER_USERNAME_1;
            string expectedFollowerEmail = EXISTING_USER_EMAIL_2;

            Follow actualFollow = await _followService.FollowUser(expectedUserUsername, expectedFollowerEmail);
            string actualUserUsername = actualFollow.User.Username;
            string actualFollowerEmail = actualFollow.FollowerEmail;

            Assert.AreEqual(expectedUserUsername, actualUserUsername);
            Assert.AreEqual(expectedFollowerEmail, actualFollowerEmail);
        }

        [Test]
        public async Task FollowUser_WithExistingUsers_AddsFollowToDatabase()
        {
            string expectedUserEmail = EXISTING_USER_EMAIL_1;
            string expectedUserUsername = EXISTING_USER_USERNAME_1;
            string expectedFollowerEmail = EXISTING_USER_EMAIL_2;

            Follow newFollow = await _followService.FollowUser(expectedUserUsername, expectedFollowerEmail);
            Follow actualFollow = GetDbContext().Follows.Find(expectedUserEmail, expectedFollowerEmail);

            Assert.IsNotNull(actualFollow);
        }

        [Test]
        public void FollowUser_WithExistingFollow_ThrowsInvalidOperationException()
        {
            string expectedUserUsername = EXISTING_FOLLOW_USER_USERNAME;
            string expectedFollowerEmail = EXISTING_FOLLOW_FOLLOWER_EMAIL;

            InvalidOperationException actualException = Assert.ThrowsAsync<InvalidOperationException>(() => _followService.FollowUser(expectedUserUsername, expectedFollowerEmail));
        }

        [Test]
        public void FollowUser_WithNonExistingUsername_ThrowsEntityNotFoundExceptionWithUsernameAndUserType()
        {
            string expectedUserUsername = NON_EXISTING_USER_USERNAME;
            Type expectedEntityType = typeof(User);

            EntityNotFoundException<string> actualException = Assert.ThrowsAsync<EntityNotFoundException<string>>(() => _followService.FollowUser(expectedUserUsername, EXISTING_FOLLOW_FOLLOWER_EMAIL));
            string actualUserUsername = actualException.EntityId;
            Type actualEntityType = actualException.EntityType;

            Assert.AreEqual(expectedUserUsername, actualUserUsername);        
            Assert.AreEqual(expectedEntityType, actualEntityType);
        }

        [Test]
        public void FollowUser_WithMatchingUserAndFollower_ThrowsOwnProfileFollowExceptionWithUserEmail()
        {
            string expectedUserEmail = EXISTING_USER_EMAIL_1;

            OwnProfileFollowException actualException = Assert.ThrowsAsync<OwnProfileFollowException>(() => _followService.FollowUser(EXISTING_USER_USERNAME_1, expectedUserEmail));
            string actualUserEmail = actualException.UserEmail;

            Assert.AreEqual(expectedUserEmail, actualUserEmail);        
        }

        [Test]
        public async Task UnfollowUser_WithExistingFollow_ReturnsTrue()
        {
            bool actual = await _followService.UnfollowUser(EXISTING_FOLLOW_USER_USERNAME, EXISTING_FOLLOW_FOLLOWER_EMAIL);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task UnfollowUser_WithNonExistingFollow_ReturnsFalse()
        {
            bool actual = await _followService.UnfollowUser(EXISTING_USER_USERNAME_1, EXISTING_USER_EMAIL_2);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task UnfollowUser_WithNonExistingUsername_ReturnsFalse()
        {
            bool actual = await _followService.UnfollowUser(NON_EXISTING_USER_USERNAME, EXISTING_USER_EMAIL_2);

            Assert.IsFalse(actual);
        }

        protected override void Seed(ShowNTellDbContext context)
        {
            context.Users.Add(new User
            {
                Email = EXISTING_USER_EMAIL_1,
                Username = EXISTING_USER_USERNAME_1
            });

            context.Users.Add(new User
            {
                Email = EXISTING_USER_EMAIL_2,
                Username = EXISTING_USER_USERNAME_2
            });

            context.Users.Add(new User
            {
                Email = EXISTING_FOLLOW_USER_EMAIL,
                Username = EXISTING_FOLLOW_USER_USERNAME
            });

            context.Users.Add(new User
            {
                Email = EXISTING_FOLLOW_FOLLOWER_EMAIL,
                Username = EXISTING_FOLLOW_FOLLOWER_USERNAME
            });

            context.Follows.Add(new Follow()
            {
                UserEmail = EXISTING_FOLLOW_USER_EMAIL,
                FollowerEmail = EXISTING_FOLLOW_FOLLOWER_EMAIL
            });
        }
    }
}
