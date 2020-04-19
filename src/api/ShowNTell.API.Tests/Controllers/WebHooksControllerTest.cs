using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Services.EventGridImageBlobDeletes;
using ShowNTell.API.Services.EventGridValidations;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Exceptions;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class WebHooksControllerTest : ControllerTest<WebHooksController>
    {
        private Mock<IEventGridImageBlobDeleteService> _mockBlobDeleteService;
        private Mock<IEventGridValidationService> _mockValidationService;
        private WebHooksController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockBlobDeleteService = new Mock<IEventGridImageBlobDeleteService>();
            _mockValidationService = new Mock<IEventGridValidationService>();
            _controller = new WebHooksController(_mockBlobDeleteService.Object, _mockValidationService.Object, _logger);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithNoEvents_ReturnsBadRequest()
        {
            EventGridEvent[] events = new EventGridEvent[]{};
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithValidValidationRequest_ReturnsOk()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.EventGridSubscriptionValidationEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockValidationService.Setup(s => s.Validate(gridEvent)).Returns(new SubscriptionValidationResponse());
            Type expectedType = typeof(OkObjectResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithInvalidValidationRequest_ReturnsBadRequest()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.EventGridSubscriptionValidationEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockValidationService.Setup(s => s.Validate(gridEvent)).Throws(new Exception());
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithValidImageBlobDeleteEventAndValidToken_ReturnsNoContent()
        {
            string token = "valid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, token);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithInvalidImageBlobDeleteEventAndValidToken_ReturnsNotFound()
        {
            string token = "valid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ReturnsAsync(false);
            Type expectedType = typeof(NotFoundResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, token);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithInvalidToken_ReturnsUnauthorized()
        {
            string token = "invalid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ThrowsAsync(new InvalidTokenException(token));
            Type expectedType = typeof(UnauthorizedResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, token);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task HandleImageBlobDelete_WithUnknownEvent_ReturnsBadRequest()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = It.IsAny<string>() };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.HandleImageBlobDelete(events, It.IsAny<string>());

            Assert.IsAssignableFrom(expectedType, actual);
        }
    }
}