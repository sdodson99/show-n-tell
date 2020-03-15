using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFLikeServiceTest
    {
        private const int _existingImagePostId = 1;
        private const int _nonExistingImagePostId = 1000;
        private const string _existingUserEmail = "test@gmail.com";
        private const string _nonExistingUserEmail = "fake@gmail.com";
        private const int _likedImagePostId = 100;
        private const string _likedImagePostLikerEmail = "like@gmail.com";
        private const string _likedImagePostOwnerEmail = "user@gmail.com";

        private ShowNTellDbContext _context;
        private EFLikeService _likeService;

        [SetUp]
        public void SetUp()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new ShowNTellDbContext(options);
            _context.Users.AddRange(GetUsers());
            _context.ImagePosts.AddRange(GetImagePosts());
            _context.SaveChanges();

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(_context);

            _likeService = new EFLikeService(contextFactory.Object);
        }

        [Test]
        public async Task LikeImagePost_WithExistingImagePostAndUser_ReturnsLike()
        {
            int expectedImagePostId = _existingImagePostId;
            string expectedEmail = _existingUserEmail;

            Like actualLike = await _likeService.LikeImagePost(expectedImagePostId, expectedEmail);
            int actualImagePostId = actualLike.ImagePostId;
            string actualEmail = actualLike.UserEmail;

            Assert.AreEqual(expectedImagePostId, actualImagePostId);
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void LikeImagePost_WithNonExistingImagePost_ThrowsEntityNotFoundExceptionForImagePostId()
        {
            int expectedImagePostId = _nonExistingImagePostId;
            string expectedEmail = _existingUserEmail;

            EntityNotFoundException<int> exception = Assert.ThrowsAsync<EntityNotFoundException<int>>(async () => 
            {
                await _likeService.LikeImagePost(expectedImagePostId, expectedEmail);
            });
            int actualImagePostId = exception.EntityId;

            Assert.AreEqual(expectedImagePostId, actualImagePostId);
        }

        [Test]
        public void LikeImagePost_WithNonExistingUserEmail_ThrowsEntityNotFoundExceptionForUserEmail()
        {
            int expectedImagePostId = _existingImagePostId;
            string expectedEmail = _nonExistingUserEmail;

            EntityNotFoundException<string> exception = Assert.ThrowsAsync<EntityNotFoundException<string>>(async () => 
            {
                await _likeService.LikeImagePost(expectedImagePostId, expectedEmail);
            });
            string actualEmail = exception.EntityId;

            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void LikeImagePost_WithAlreadyLikingUserEmail_ThrowsDuplicateLikeExceptionForImagePostAndEmail()
        {
            int expectedImagePostId = _likedImagePostId;
            string expectedEmail = _likedImagePostLikerEmail;

            DuplicateLikeException exception = Assert.ThrowsAsync<DuplicateLikeException>(async () => 
            {
                await _likeService.LikeImagePost(expectedImagePostId, expectedEmail);
            });
            int actualImagePostId = exception.LikedImagePost.Id;
            string actualEmail = exception.LikerEmail;

            Assert.AreEqual(expectedImagePostId, actualImagePostId);
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void LikeImagePost_WithImagePostUserEmail_ThrowsOwnImageLikeExceptionForImagePostAndEmail()
        {
            int expectedImagePostId = _likedImagePostId;
            string expectedEmail = _likedImagePostOwnerEmail;

            OwnImagePostLikeException exception = Assert.ThrowsAsync<OwnImagePostLikeException>(async () => 
            {
                await _likeService.LikeImagePost(expectedImagePostId, expectedEmail);
            });
            int actualImagePostId = exception.LikedImagePost.Id;
            string actualEmail = exception.LikerEmail;

            Assert.AreEqual(expectedImagePostId, actualImagePostId);
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public async Task UnlikeImagePost_WithExistingLike_ReturnsTrue()
        {
            bool actual = await _likeService.UnlikeImagePost(_likedImagePostId, _likedImagePostLikerEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task UnlikeImagePost_WithExistingLike_ReturnsFalse()
        {
            bool actual = await _likeService.UnlikeImagePost(_likedImagePostId, _nonExistingUserEmail);

            Assert.IsFalse(actual);
        }

        private IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User() 
                {
                    Email = _existingUserEmail
                },
                new User() 
                {
                    Email = _likedImagePostLikerEmail
                },
                new User()
                {
                    Email = _likedImagePostOwnerEmail
                }
            };
        }

        private IEnumerable<ImagePost> GetImagePosts()
        {
            return new List<ImagePost>()
            {
                new ImagePost() 
                {
                    Id = _existingImagePostId
                },
                new ImagePost()
                {
                    Id = _likedImagePostId,
                    UserEmail = _likedImagePostOwnerEmail,
                    Likes = new List<Like>()
                    {
                        new Like()
                        {
                            UserEmail = _likedImagePostLikerEmail
                        }
                    }
                }
            };
        }
    }
}