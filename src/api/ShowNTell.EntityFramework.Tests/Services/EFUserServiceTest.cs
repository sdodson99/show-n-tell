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
    public class EFUserServiceTest
    {
        private const string EXISTING_EMAIL = "test@gmail.com";
        private const string NON_EXISTING_EMAIL = "missing@gmail.com";
        private EFUserService _userService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString());
            ShowNTellDbContext context = new ShowNTellDbContext(optionsBuilder.Options);
            context.Users.Add(GetUser());
            context.SaveChanges();

            Mock<IShowNTellDbContextFactory> mockContextFactory = new Mock<IShowNTellDbContextFactory>();
            mockContextFactory.Setup(c => c.CreateDbContext()).Returns(context);

            _userService = new EFUserService(mockContextFactory.Object);
        }

        [Test]
        public async Task GetByEmail_WithExistingEmail_ReturnsUserWithEmail()
        {
            string expectedEmail = EXISTING_EMAIL;

            User user = await _userService.GetByEmail(expectedEmail);
            string actualEmail = user.Email;

            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public async Task GetByEmail_WithNonExistingEmail_ReturnsNull()
        {
            User user = await _userService.GetByEmail(NON_EXISTING_EMAIL);

            Assert.IsNull(user);
        }

        [Test]
        public async Task Create_WithNonExistingEmail_ReturnsNewUserWithEmail()
        {
            string expectedEmail = NON_EXISTING_EMAIL;
            User newUser = new User()
            {
                Email = expectedEmail
            };

            User createdUser = await _userService.Create(newUser);
            string actualEmail = createdUser.Email;

            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void Create_WithExistingEmail_ThrowsArgumentExceptionForEmail()
        {
            string expectedInvalidParamName = "email";
            User newUser = new User()
            {
                Email = EXISTING_EMAIL
            };

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(() => _userService.Create(newUser));
            string actualInvalidParamName = exception.ParamName;

            Assert.AreEqual(expectedInvalidParamName, actualInvalidParamName);
        }

        private User GetUser()
        {
            return new User()
            {
                Email = EXISTING_EMAIL
            };
        }
    }
}