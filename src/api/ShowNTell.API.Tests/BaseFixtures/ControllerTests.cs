using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Models.MappingProfiles;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Tests.BaseFixtures
{
    [TestFixture]
    public abstract class ControllerTests<TControllerType>
    {
        protected IMapper _mapper;
        protected ILogger<TControllerType> _logger;
        protected ICurrentUserService _currentUserService;

        protected User CurrentUser
        {
            get
            {
                return new User
                {
                    Email = "test@gmail.com",
                    Username = "testuser"
                };
            }
        }

        [SetUp]
        public void BaseSetUp()
        {
            _mapper = new MapperFactory().CreateMapper();

            Mock<ILogger<TControllerType>> mockLogger = new Mock<ILogger<TControllerType>>();
            _logger = mockLogger.Object;

            Mock<ICurrentUserService> mockCurrentUserService = new Mock<ICurrentUserService>();
            mockCurrentUserService.Setup(s => s.GetCurrentUser(It.IsAny<HttpContext>())).Returns(CurrentUser);
            _currentUserService = mockCurrentUserService.Object;
        }
    }
}