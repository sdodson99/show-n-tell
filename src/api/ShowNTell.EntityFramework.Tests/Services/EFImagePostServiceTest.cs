using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    public class EFImagePostServiceTest
    {
        private const int _validUserCount = 3;
        private const string _validUserEmail = "test@gmail.com";
        private const string _invalidEmail = "badtest@gmail.com";
        private const int _existingId = 1000;
        private const int _nonExistingId = 1001;

        private ShowNTellDbContext _context;
        private EFImagePostService _imagePostService;

        [SetUp]
        public void Setup()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new ShowNTellDbContext(options);
            _context.ImagePosts.AddRange(GetImagePosts());
            _context.SaveChanges();

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(_context);

            _imagePostService = new EFImagePostService(contextFactory.Object);
        }

        [Test]
        public async Task GetAllByUserEmail_WithExistingRecords_ReturnsAllForEmail()
        {
            int expectedCount = _validUserCount;

            IEnumerable<ImagePost> imagePosts = await _imagePostService.GetAllByUserEmail(_validUserEmail);
            int actualCount = imagePosts.Count();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsTrue(imagePosts.All(p => p.User.Email == _validUserEmail));
        }

        [Test]
        public async Task GetById_WithExistingId_ReturnsImagePostWithId()
        {
            int expectedId = _existingId;

            ImagePost imagePost = await _imagePostService.GetById(expectedId);
            int actualId = imagePost.Id;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public async Task GetById_WithNonExistingId_ReturnsNull()
        {
            ImagePost imagePost = await _imagePostService.GetById(_nonExistingId);

            Assert.IsNull(imagePost);
        }

        [Test]
        public async Task Create_WithImagePost_ReturnsImagePostWithNewId()
        {
            ImagePost newImagePost = new ImagePost();
            int oldImagePostId = newImagePost.Id;

            ImagePost createdImagePost = await _imagePostService.Create(newImagePost);
            int newImagePostId = createdImagePost.Id;

            Assert.AreNotEqual(oldImagePostId, newImagePostId);
        }

        [Test]
        public async Task Update_WithExistingImagePostId_ReturnsUpdatedImagePostWithId()
        {
            int expectedId = _existingId;
            ImagePost storedImagePost = _context.ImagePosts.Find(expectedId);
            string oldDescription = storedImagePost.Description;
            string newDescription = "Updated description";
            ImagePost newImagePost = new ImagePost()
            {
                Description = newDescription
            };

            ImagePost updatedImagePost = await _imagePostService.Update(expectedId, newImagePost);
            int actualId = updatedImagePost.Id;
            string updatedDescription = updatedImagePost.Description;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreNotEqual(oldDescription, updatedDescription);
        }

        [Test]
        public void Update_WithNonExistingImagePostId_ThrowsEntityNotFoundExceptionWithId()
        {
            int expectedId = _nonExistingId;
            ImagePost createdImagePost = new ImagePost();

            EntityNotFoundException<int> exception = Assert.ThrowsAsync<EntityNotFoundException<int>>(() => 
                _imagePostService.Update(expectedId, createdImagePost));
            int actualId = exception.EntityId;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public async Task Delete_WithExistingImagePostId_ReturnsTrue()
        {
            bool success = await _imagePostService.Delete(_existingId);
            
            Assert.IsTrue(success);
        }

        [Test]
        public async Task Delete_WithNonExistingImagePostId_ReturnsFalse()
        {
            bool success = await _imagePostService.Delete(_nonExistingId);

            Assert.IsFalse(success);
        }

        private IEnumerable<ImagePost> GetImagePosts()
        {
            List<ImagePost> imagePosts = new List<ImagePost>();

            // Add expected amount of valid user image posts.
            for (int i = 0; i < _validUserCount; i++)
            {
                imagePosts.Add(new ImagePost()
                {
                    User = GetValidUser()
                });
            }

            // Add image posts with an expected id.
            imagePosts.Add(new ImagePost()
            {
                Id = _existingId,
            });

            // Add some invalid user image posts.
            imagePosts.AddRange(new List<ImagePost>
            {
                new ImagePost()
                {
                    User = GetInvalidUser()
                },
                new ImagePost()
                {
                    User = GetInvalidUser()
                },
                new ImagePost()
                {
                    User = GetInvalidUser()
                }
            });

            return imagePosts;
        }

        private User GetValidUser()
        {
            return new User()
            {
                Email = _validUserEmail
            };
        }

        private User GetInvalidUser()
        {
            return new User()
            {
                Email = _invalidEmail
            };
        }
    }
}