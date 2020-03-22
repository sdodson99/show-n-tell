using System;
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
    public class EFCommentServiceTest
    {
        private string _databaseName;

        private EFCommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _databaseName = Guid.NewGuid().ToString();

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(() => GetDbContext());

            _commentService = new EFCommentService(contextFactory.Object);
        }   

        [Test]
        public async Task Create_WithValidComment_ReturnsCreatedCommentWithNewIdAndExpectedContent()
        {
            Comment newComment = new Comment()
            {
                Content = "hello world"
            };
            string expectedContent = newComment.Content;
            int nonExpectedId = newComment.Id;

            Comment createdComment = await _commentService.Create(newComment);
            string actualContent = createdComment.Content;
            int actualId = createdComment.Id;

            Assert.AreEqual(expectedContent, actualContent);
            Assert.AreNotEqual(nonExpectedId, actualId);
        }

        private ShowNTellDbContext GetDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(_databaseName).Options;

            ShowNTellDbContext context = new ShowNTellDbContext(options);

            return context;
        }
    }
}