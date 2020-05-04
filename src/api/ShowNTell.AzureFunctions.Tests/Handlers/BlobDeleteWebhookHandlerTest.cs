using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShowNTell.AzureFunctions.Exceptions;
using ShowNTell.AzureFunctions.Handlers;
using ShowNTell.AzureFunctions.Services.EventGridImageBlobDeletes;
using ShowNTell.AzureFunctions.Services.EventGridValidations;

namespace ShowNTell.AzureFunctions.Tests.Handlers
{
    [TestFixture]
    public class BlobDeleteWebhookHandlerTest
    {
        private Mock<IEventGridImageBlobDeleteService> _mockBlobDeleteService;
        private Mock<IEventGridValidationService> _mockValidationService;
        private ILogger _logger;
        private BlobDeleteWebhookHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockBlobDeleteService = new Mock<IEventGridImageBlobDeleteService>();
            _mockValidationService = new Mock<IEventGridValidationService>();
            _logger = new Mock<ILogger>().Object;
            _handler = new BlobDeleteWebhookHandler(_mockValidationService.Object, _mockBlobDeleteService.Object);
        }

        [Test]
        public async Task Handle_WithNoEvents_ReturnsBadRequest()
        {
            EventGridEvent[] events = new EventGridEvent[]{};
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _handler.Handle(events, It.IsAny<string>(), _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithValidValidationRequest_ReturnsOk()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.EventGridSubscriptionValidationEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockValidationService.Setup(s => s.Validate(gridEvent)).Returns(new SubscriptionValidationResponse());
            Type expectedType = typeof(OkObjectResult);

            IActionResult actual = await _handler.Handle(events, It.IsAny<string>(), _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithInvalidValidationRequest_ReturnsBadRequest()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.EventGridSubscriptionValidationEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockValidationService.Setup(s => s.Validate(gridEvent)).Throws(new Exception());
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _handler.Handle(events, It.IsAny<string>(), _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithValidImageBlobDeleteEventAndValidToken_ReturnsNoContent()
        {
            string token = "valid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _handler.Handle(events, token, _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithInvalidImageBlobDeleteEventAndValidToken_ReturnsNotFound()
        {
            string token = "valid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ReturnsAsync(false);
            Type expectedType = typeof(NotFoundResult);

            IActionResult actual = await _handler.Handle(events, token, _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithInvalidToken_ReturnsUnauthorized()
        {
            string token = "invalid-token";
            EventGridEvent gridEvent = new EventGridEvent() { EventType = EventTypes.StorageBlobDeletedEvent };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            _mockBlobDeleteService.Setup(s => s.DeleteImagePost(gridEvent, token)).ThrowsAsync(new InvalidTokenException(token));
            Type expectedType = typeof(UnauthorizedResult);

            IActionResult actual = await _handler.Handle(events, token, _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Handle_WithUnknownEvent_ReturnsBadRequest()
        {
            EventGridEvent gridEvent = new EventGridEvent() { EventType = It.IsAny<string>() };
            EventGridEvent[] events = new EventGridEvent[]{ gridEvent };
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _handler.Handle(events, It.IsAny<string>(), _logger);

            Assert.IsAssignableFrom(expectedType, actual);
        }
    }
}