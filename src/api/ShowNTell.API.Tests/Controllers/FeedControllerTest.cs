using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Models.MappingProfiles;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class FeedControllerTest : ControllerTest<FeedController>
    {
        private Mock<IFeedService> _mockFeedService;
        private FeedController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new FeedController(CreateFeedService(), _mapper, _logger, _currentUserService);
        }

        [Test]
        public async Task GetFeed_ReturnsOkAndCallsGetFeed()
        {
            Type expectedType = typeof(OkObjectResult);

            ActionResult<IEnumerable<ImagePostResponse>> actual = await _controller.GetFeed();
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
            _mockFeedService.Verify();
        }

        private IFeedService CreateFeedService()
        {
            _mockFeedService = new Mock<IFeedService>();

            _mockFeedService.Setup(s => s.GetFeed(It.IsAny<string>())).ReturnsAsync(new List<ImagePost>()).Verifiable();

            return _mockFeedService.Object;
        }
    }
}