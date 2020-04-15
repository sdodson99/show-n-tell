using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Exceptions;
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
            Comment newComment = new Comment() { Content = "hello world" };
            string expectedContent = newComment.Content;
            int nonExpectedId = newComment.Id;

            Comment createdComment = await _commentService.Create(newComment);
            string actualContent = createdComment.Content;
            int actualId = createdComment.Id;

            Assert.AreEqual(expectedContent, actualContent);
            Assert.AreNotEqual(nonExpectedId, actualId);
        }

        [Test]
        public async Task Update_WithExistingComment_ReturnsCommentWithNewContent()
        {
            Comment comment = new Comment();
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();
            string expectedContent = "new content";

            Comment actualComment = await _commentService.Update(comment.Id, expectedContent);
            string actualContent = actualComment.Content;

            Assert.AreEqual(expectedContent, actualContent);
        }

        [Test]
        public async Task Update_WithExistingComment_UpdatesCommentWithNewContent()
        {
            Comment comment = new Comment();
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();
            string expectedContent = "new content";

            await _commentService.Update(comment.Id, expectedContent);

            Comment actualComment = context.Comments.FirstOrDefault(c => c.Id == comment.Id);
            string actualContent = actualComment.Content;
            Assert.AreEqual(expectedContent, actualContent);
        }

        [Test]
        public void Update_WithNonExistingComment_ThrowsEntityNotFoundExceptionForCommentIdAndType()
        {
            int expectedId = 1000;
            Type expectedType = typeof(Comment);

            EntityNotFoundException<int> actualException = Assert.ThrowsAsync<EntityNotFoundException<int>>(() => _commentService.Update(expectedId, It.IsAny<string>()));
            int actualId = actualException.EntityId;
            Type actualType = actualException.EntityType;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedType, actualType);
        }

        [Test]
        public async Task Delete_WithExistingComment_ReturnsTrue()
        {
            Comment comment = new Comment();
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.Delete(comment.Id);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task Delete_WithExistingComment_DeletesComment()
        {
            Comment comment = new Comment();
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.Delete(comment.Id);

            Assert.IsEmpty(context.Comments);
        }

        [Test]
        public async Task Delete_WithNonExistingComment_ReturnsFalse()
        {
            bool actual = await _commentService.Delete(It.IsAny<int>());

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task IsCommentOwner_WithExistingCommentOwnedByUser_ReturnsTrue()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment()
            {
                User = new User { Email = userEmail }
            };
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.IsCommentOwner(comment.Id, userEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task IsCommentOwner_WithExistingCommentNotOwnedByUser_ReturnsFalse()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment();
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.IsCommentOwner(comment.Id, userEmail);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task IsCommentOwner_WithNonExistingComment_ReturnsFalse()
        {
            bool actual = await _commentService.IsCommentOwner(It.IsAny<int>(), It.IsAny<string>());

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task CanDelete_WithExistingCommentOwnedByUser_ReturnsTrue()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment()
            {
                User = new User { Email = userEmail },
                ImagePost = new ImagePost()
            };
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.CanDelete(comment.Id, userEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task CanDelete_WithExistingCommentNotOwnedByUser_ReturnsFalse()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment()
            {
                ImagePost = new ImagePost()
            };
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.CanDelete(comment.Id, userEmail);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task CanDelete_WithExistingCommentOnImagePostOwnedByUser_ReturnsTrue()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment()
            {
                ImagePost = new ImagePost() { User = new User { Email = userEmail }}
            };
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.CanDelete(comment.Id, userEmail);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task CanDelete_WithExistingCommentOnImagePostNotOwnedByUser_ReturnsFalse()
        {
            string userEmail = "test@gmail.com";
            Comment comment = new Comment()
            {
                ImagePost = new ImagePost()
            };
            ShowNTellDbContext context = GetDbContext();
            context.Add(comment);
            context.SaveChanges();

            bool actual = await _commentService.CanDelete(comment.Id, userEmail);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task CanDelete_WithNonExistingComment_ReturnsFalse()
        {
            bool actual = await _commentService.CanDelete(It.IsAny<int>(), It.IsAny<string>());

            Assert.IsFalse(actual);
        }

        protected override void Seed(ShowNTellDbContext context) { }
    }
}