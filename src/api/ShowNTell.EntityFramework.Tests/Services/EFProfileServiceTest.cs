using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
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
    public class EFProfileServiceTest : EFTest
    {
        private const int EXISTING_USER_IMAGE_POST_COUNT = 3;
        private const string EXISTING_USER_EMAIL = "test@gmail.com";
        private const string EXISTING_USER_USERNAME = "testuser";
        private const string NON_EXISTING_USER_USERNAME = "fakeuser";
        private const string ALTERNATE_USER_EMAIL = "alternate@gmail.com";

        private EFProfileService _profileService;

        [SetUp]
        public void Setup()
        {
            _profileService = new EFProfileService(_contextFactory);
        }

        [Test]
        public async Task GetProfile_WithExistingUser_ReturnsProfileForUsername()
        {
            string expectedUsername = EXISTING_USER_USERNAME;

            User actualProfile = await _profileService.GetProfile(expectedUsername);
            string actualUsername = actualProfile.Username;

            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public async Task GetProfile_WithExistingUser_ReturnsNonNullNavigationProperties()
        {
            User actualProfile = await _profileService.GetProfile(EXISTING_USER_USERNAME);

            Assert.IsNotNull(actualProfile.ImagePosts);
            Assert.IsNotNull(actualProfile.Followers);
            Assert.IsNotNull(actualProfile.Following);
        }

        [Test]
        public async Task GetProfile_WithNonExistingUser_ReturnsNull()
        {
            User actualProfile = await _profileService.GetProfile(NON_EXISTING_USER_USERNAME);

            Assert.IsNull(actualProfile);
        }

        [Test]
        public async Task GetImagePosts_WithExistingUser_ReturnsAllForUsername()
        {
            int expectedCount = EXISTING_USER_IMAGE_POST_COUNT;
            string expectedUsername = EXISTING_USER_USERNAME;

            IEnumerable<ImagePost> imagePosts = await _profileService.GetImagePosts(expectedUsername);
            int actualCount = imagePosts.Count();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsTrue(imagePosts.All(p => p.User.Username == expectedUsername));
        }

        [Test]
        public async Task GetImagePosts_WithNonExistingUser_ReturnsEmptyCollectionOfImagePosts()
        {
            IEnumerable<ImagePost> actualImagePosts = await _profileService.GetImagePosts(NON_EXISTING_USER_USERNAME);

            Assert.IsEmpty(actualImagePosts);
        }

        protected override void Seed(ShowNTellDbContext context)
        {
            context.Users.Add(new User()
            {
                Email = EXISTING_USER_EMAIL,
                Username = EXISTING_USER_USERNAME
            });

            for (int i = 0; i < EXISTING_USER_IMAGE_POST_COUNT; i++)
            {
                context.ImagePosts.Add(new ImagePost()
                {
                    UserEmail = EXISTING_USER_EMAIL
                });
            }

            // Add an alternate user image post.
            context.Users.Add(new User()
            {
                Email = ALTERNATE_USER_EMAIL
            });

            context.ImagePosts.Add(new ImagePost()
            {
                UserEmail = ALTERNATE_USER_EMAIL
            });
        }
    }
}
