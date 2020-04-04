using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    public class EFImagePostServiceTest : EFTest
    {
        private const int _validUserCount = 3;
        private const string _validUserEmail = "test@gmail.com";
        private const string _validUsername = "test";
        private const string _invalidEmail = "badtest@gmail.com";
        private const string _invalidUsername = "badtest@gmail.com";
        private const int _existingId = 1000;
        private const string _existingEmail = "existing@gmail.com";
        private const string _existingUri = "C:/images/test.jpg";
        private const int _nonExistingId = 1001;
        private const string _existingTagContent = "Funny";

        private EFImagePostService _imagePostService;

        [SetUp]
        public void Setup()
        {
            _imagePostService = new EFImagePostService(_contextFactory);
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
        public async Task Create_WithImagePostWithNewTags_SavesTagsInDatabase()
        {
            IEnumerable<string> expectedTags = new []{ "Fun", "Cool" };
            ImagePost newImagePost = new ImagePost()
            {
                Tags = expectedTags.Select((t) => new ImagePostTag(){ Tag = new Tag() { Content = t } }).ToList()
            };

            ImagePost createdImagePost = await _imagePostService.Create(newImagePost);
            IEnumerable<string> actualTags = GetDbContext().Tags.Select(t => t.Content);

            foreach (string expectedTag in expectedTags)
            {
                Assert.Contains(expectedTag, actualTags.ToList());
            }
        }

        [Test]
        public async Task Create_WithImagePostWithNewAndExistingTags_DoesNotDuplicateExistingTag()
        {
            int expectedExistingTagCount = 1;
            IEnumerable<string> newTags = new []{ "Fun", _existingTagContent };
            ImagePost newImagePost = new ImagePost()
            {
                Tags = newTags.Select((t) => new ImagePostTag(){ Tag = new Tag() { Content = t } }).ToList()
            };

            ImagePost createdImagePost = await _imagePostService.Create(newImagePost);
            int actualExistingTagCount = GetDbContext().Tags.Count(t => t.Content == _existingTagContent);

            Assert.AreEqual(expectedExistingTagCount, actualExistingTagCount);
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndDescription_ReturnsUpdatedImagePostWithIdAndDescription()
        {
            int expectedId = _existingId;
            string expectedDescription = "Updated description";

            ImagePost updatedImagePost = await _imagePostService.Update(expectedId, expectedDescription);
            int actualId = updatedImagePost.Id;
            string actualDescription = updatedImagePost.Description;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndDescription_ChangesStoredImagePostDescription()
        {
            string expectedDescription = "New description";

            ImagePost updatedImagePost = await _imagePostService.Update(_existingId, expectedDescription);
            string actualDescription = GetDbContext().ImagePosts.Find(_existingId).Description;

            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndDescription_DoesNotChangeStoredImagePostUri()
        {
            string expectedUri = GetDbContext().ImagePosts.Find(_existingId).ImageUri;

            ImagePost updatedImagePost = await _imagePostService.Update(_existingId, "New description");
            string actualUri = GetDbContext().ImagePosts.Find(_existingId).ImageUri;

            Assert.AreEqual(expectedUri, actualUri);
        }

        [Test]
        public void Update_WithNonExistingImagePostIdAndDescription_ThrowsEntityNotFoundExceptionWithId()
        {
            int expectedId = _nonExistingId;
            ImagePost createdImagePost = new ImagePost();

            EntityNotFoundException<int> exception = Assert.ThrowsAsync<EntityNotFoundException<int>>(() => 
                _imagePostService.Update(expectedId, "Updated Description"));
            int actualId = exception.EntityId;

            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndNewAndExistingTags_ReturnsUpdatedImagePostWithTags()
        {
            int expectedId = _existingId;
            List<Tag> expectedTags = new List<Tag>()
            {
                new Tag() { Content = "awesome" },
                new Tag() { Content = "wow" },
                new Tag() { Content = _existingTagContent }
            };

            ImagePost updatedImagePost = await _imagePostService.Update(expectedId, string.Empty, expectedTags);
            IEnumerable<Tag> actualTags = updatedImagePost.Tags.Select(t => t.Tag);

            Assert.IsTrue(expectedTags.All(e => actualTags.Any(a => e.Content == a.Content)));
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndNewAndExistingTags_SavesImagePostWithUpdatedTags()
        {
            List<Tag> expectedTags = new List<Tag>()
            {
                new Tag() { Content = "awesome" },
                new Tag() { Content = "wow" },
                new Tag() { Content = _existingTagContent }
            };
            int expectedTagCount = 3;

            ImagePost updatedImagePost = await _imagePostService.Update(_existingId, string.Empty, expectedTags);
            IEnumerable<Tag> actualTags = GetDbContext().ImagePosts
                .Include(p => p.Tags)
                    .ThenInclude(t => t.Tag)
                .FirstOrDefault(p => p.Id == _existingId)
                .Tags.Select(t => t.Tag);
            int actualTagCount = actualTags.Count();

            Assert.IsTrue(expectedTags.All(e => actualTags.Any(a => e.Content == a.Content)));
            Assert.AreEqual(expectedTagCount, actualTagCount);
        }

        [Test]
        public async Task Update_WithExistingImagePostIdAndNewAndExistingTags_DoesNotDuplicateExistingTag()
        {
            int expectedExistingTagCount = 1;
            List<Tag> expectedTags = new List<Tag>()
            {
                new Tag() { Content = "awesome" },
                new Tag() { Content = "wow" },
                new Tag() { Content = _existingTagContent }
            };

            ImagePost updatedImagePost = await _imagePostService.Update(_existingId, string.Empty, expectedTags);
            int actualExistingTagCount = GetDbContext().Tags.Count(t => t.Content == _existingTagContent);

            Assert.AreEqual(expectedExistingTagCount, actualExistingTagCount);
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

        [Test]
        public async Task IsAuthor_ForImagePostWithCorrectAuthorEmail_ReturnsTrue()
        {
            bool actual = await _imagePostService.IsAuthor(_existingId, _existingEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task IsAuthor_ForImagePostWithIncorrectAuthorEmail_ReturnsFalse()
        {
            bool actual = await _imagePostService.IsAuthor(_existingId, _invalidEmail);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task IsAuthor_ForNonExistingImagePost_ReturnsFalse()
        {
            bool actual = await _imagePostService.IsAuthor(_nonExistingId, _existingEmail);

            Assert.IsFalse(actual);
        }

        protected override void Seed(ShowNTellDbContext context)
        {
            context.Users.Add(GetValidUser());
            context.Users.Add(GetInvalidUser());
            context.Tags.Add(new Tag() { Content = _existingTagContent });
            context.ImagePosts.AddRange(GetImagePosts());
        }

        private IEnumerable<ImagePost> GetImagePosts()
        {
            List<ImagePost> imagePosts = new List<ImagePost>();

            // Add expected amount of valid user image posts.
            for (int i = 0; i < _validUserCount; i++)
            {
                imagePosts.Add(new ImagePost()
                {
                    UserEmail = _validUserEmail,
                });
            }

            // Add image posts with an expected id.
            imagePosts.Add(new ImagePost()
            {
                Id = _existingId,
                UserEmail = _existingEmail,
                ImageUri = _existingUri,
                Tags = new List<ImagePostTag>
                {
                    new ImagePostTag()
                    {
                        Tag = new Tag()
                        {
                            Content = "cat",
                        }
                    },
                    new ImagePostTag()
                    {
                        Tag = new Tag()
                        {
                            Content = "dog",
                        }
                    },
                    new ImagePostTag()
                    {
                        Tag = new Tag()
                        {
                            Content = "rabbit",
                        }
                    }
                }
            });

            // Add some invalid user image posts.
            imagePosts.AddRange(new List<ImagePost>
            {
                new ImagePost()
                {
                    UserEmail = _invalidEmail
                },
                new ImagePost()
                {
                    UserEmail = _invalidEmail
                },
                new ImagePost()
                {
                    UserEmail = _invalidEmail
                }
            });

            return imagePosts;
        }

        private User GetValidUser()
        {
            return new User()
            {
                Email = _validUserEmail,
                Username = _validUsername
            };
        }

        private User GetInvalidUser()
        {
            return new User()
            {
                Email = _invalidEmail,
                Username = _invalidUsername
            };
        }
    }
}