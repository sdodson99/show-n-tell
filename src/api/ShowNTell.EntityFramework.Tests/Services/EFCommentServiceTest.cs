using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using ShowNTell.EntityFramework.Tests.BaseFixtures;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFCommentServiceTest : EFTest
    {
        private EFCommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _commentService = new EFCommentService(_contextFactory);
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

        protected override void Seed(ShowNTellDbContext context){}
    }
}