using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFFeedServiceTest
    {
        private readonly DateTime[] FEED_IMAGE_POST_CREATION_DATES = new []{ DateTime.Now.AddDays(1), DateTime.Now };

        private User _existingUser;
        private string _databaseName;
        private EFFeedService _feedService;

        [SetUp]
        public void Setup() 
        {
            _databaseName = Guid.NewGuid().ToString();
            _existingUser = GetExistingUser();

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(() => GetDbContext());

            _feedService = new EFFeedService(contextFactory.Object);
        }

        [Test]
        public async Task GetFeed_WithExistingUser_ReturnsImagePostsByUsersTheyFollow()
        {
            IEnumerable<ImagePost> actualImagePosts = await _feedService.GetFeed(_existingUser.Email);

            Assert.IsNotEmpty(actualImagePosts);
            Assert.IsTrue(actualImagePosts.All(i => _existingUser.Following.Any(f => f.User.Email == i.User.Email)));
        }

        [Test]
        public async Task GetFeed_WithExistingUser_ReturnsImagePostsWithNonNullNavigationProperties()
        {
            IEnumerable<ImagePost> actualImagePosts = await _feedService.GetFeed(_existingUser.Email);
            ImagePost actualImagePost = actualImagePosts.First();

            Assert.IsNotNull(actualImagePost.User);
            Assert.IsNotNull(actualImagePost.Comments);
            Assert.IsNotNull(actualImagePost.Likes);
        }

        [Test]
        public async Task GetFeed_WithExistingUser_ReturnsImagePostsOrderedByNewestFirst()
        {
            IEnumerable<DateTime> expectedCreationDates = FEED_IMAGE_POST_CREATION_DATES;

            IEnumerable<ImagePost> actualImagePosts = await _feedService.GetFeed(_existingUser.Email);
            IEnumerable<DateTime> actualCreationDates = actualImagePosts.Select(p => p.DateCreated);

            Assert.IsTrue(expectedCreationDates.SequenceEqual(actualCreationDates));
        }

        private ShowNTellDbContext GetDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(_databaseName).Options;

            ShowNTellDbContext context = new ShowNTellDbContext(options);

            if (!context.Users.Any())
            {
                context.ImagePosts.Add(new ImagePost());
                context.ImagePosts.Add(new ImagePost());
                context.ImagePosts.Add(new ImagePost());

                context.Users.Add(GetExistingUser());
            }

            context.SaveChanges();

            return context;
        }

        private User GetExistingUser()
        {
            return new User()
            {
                Email = "existing@gmail.com",
                Following = new List<Follow>()
                {
                    new Follow()
                    {
                        User = new User()
                        {
                            Email = "following1@gmail.com",
                            ImagePosts = new List<ImagePost>()
                            {
                                new ImagePost()
                                {
                                    DateCreated = FEED_IMAGE_POST_CREATION_DATES[0]
                                }
                            }
                        }
                    },
                    new Follow()
                    {
                        User = new User()
                        {
                            Email = "following2@gmail.com",
                            ImagePosts = new List<ImagePost>()
                            {
                                new ImagePost()
                                {
                                    DateCreated = FEED_IMAGE_POST_CREATION_DATES[1]
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}