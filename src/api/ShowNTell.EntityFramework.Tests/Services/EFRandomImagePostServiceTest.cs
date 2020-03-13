using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFRandomImagePostServiceTest
    {
        private ShowNTellDbContext _context;
        private EFRandomImagePostService _randomImagePostService;

        [SetUp]
        public void Setup()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new ShowNTellDbContext(options);

            Mock<IShowNTellDbContextFactory> mockContextFactory = new Mock<IShowNTellDbContextFactory>();
            mockContextFactory.Setup(c => c.CreateDbContext()).Returns(_context);

            _randomImagePostService = new EFRandomImagePostService(mockContextFactory.Object);
        }

        [Test]
        public async Task GetRandom_WithExistingImagePosts_ReturnsImagePost()
        {
            _context.ImagePosts.Add(new ImagePost());
            await _context.SaveChangesAsync();

            ImagePost actual = await _randomImagePostService.GetRandom();

            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task GetRandom_WithNoExistingImagePosts_ReturnsNull()
        {
            ImagePost actual = await _randomImagePostService.GetRandom();

            Assert.IsNull(actual);
        }
    }
}
