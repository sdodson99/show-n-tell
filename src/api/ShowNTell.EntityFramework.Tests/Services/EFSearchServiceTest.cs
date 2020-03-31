using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.Tests.BaseFixtures;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFSearchServiceTest : EFTest
    {
        private const string DESCRIPTION_QUERY = "query";
        private const string TAG_QUERY = "beautiful";
        private const string USER_EMAIL_QUERY = "testuser";

        private int _imagePostCount;

        private EFSearchService _searchService;

        [SetUp]
        public void Setup()
        {
            _searchService = new EFSearchService(_contextFactory);
        }

        [Test]
        public async Task SearchImagePosts_WithQuery_ReturnsImagePostsWithDescriptionContainingQuery()
        {
            string query = DESCRIPTION_QUERY;

            IEnumerable<ImagePost> actualImagePosts = await _searchService.SearchImagePosts(query);

            Assert.IsTrue(actualImagePosts.All(p => p.Description.Contains(query)));
        }

        [Test]
        public async Task SearchImagePosts_WithQuery_ReturnsImagePostsWithATagContainingQuery()
        {
            string query = TAG_QUERY;

            IEnumerable<ImagePost> actualImagePosts = await _searchService.SearchImagePosts(query);

            Assert.IsTrue(actualImagePosts.All(p => p.Tags.Select(t => t.Tag.Content).Any(c => c.Contains(query))));
        }

        [Test]
        public async Task SearchImagePosts_WithQuery_ReturnsImagePostsWithUserEmailContainingQuery()
        {
            string query = USER_EMAIL_QUERY;

            IEnumerable<ImagePost> actualImagePosts = await _searchService.SearchImagePosts(query);

            Assert.IsTrue(actualImagePosts.All(p => p.UserEmail.Contains(query)));
        }

        [Test]
        public async Task SearchImagePosts_WithEmptyQuery_ReturnsAllImagePosts()
        {
            int expectedCount = _imagePostCount;
            string query = string.Empty;

            IEnumerable<ImagePost> actualImagePosts = await _searchService.SearchImagePosts(query);
            int actualCount = actualImagePosts.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        protected override void Seed(ShowNTellDbContext context)
        {
            context.Add(new ImagePost()
            {
                Description = $"this contains {DESCRIPTION_QUERY} the description"
            });

            context.Add(new ImagePost()
            {
                Description = $"i also contain {DESCRIPTION_QUERY} the description"
            });

            context.Add(new ImagePost()
            {
                UserEmail = USER_EMAIL_QUERY,
                Description = $"i do not contain the description query"
            });

            context.Add(new ImagePost()
            {
                UserEmail = USER_EMAIL_QUERY,
                Tags = new List<ImagePostTag>()
                {
                    new ImagePostTag
                    {
                        Tag = new Tag
                        {
                            Content = $"very {TAG_QUERY}"
                        }
                    }
                }
            });

            context.Add(new ImagePost()
            {
                Tags = new List<ImagePostTag>()
                {
                    new ImagePostTag
                    {
                        Tag = new Tag
                        {
                            Content = $"{TAG_QUERY}"
                        }
                    }
                }
            });

            context.Add(new ImagePost()
            {
                Tags = new List<ImagePostTag>()
                {
                    new ImagePostTag
                    {
                        Tag = new Tag
                        {
                            Content = $"random"
                        }
                    }
                }
            });

            _imagePostCount = context.ImagePosts.Count();
        }
    }
}