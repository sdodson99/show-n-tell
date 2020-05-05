using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using ShowNTell.AzureFunctions.Exceptions;
using ShowNTell.AzureFunctions.Services.EventGridImageBlobDeletes;
using ShowNTell.AzureFunctions.Services.EventGridValidations;

namespace ShowNTell.AzureFunctions.Handlers
{
    public class BlobDeleteWebhookHandler
    {
        private readonly IEventGridValidationService _eventGridValidationService;
        private readonly IEventGridImageBlobDeleteService _eventGridImageBlobDeleteService;

        public BlobDeleteWebhookHandler(IEventGridValidationService eventGridValidationService, IEventGridImageBlobDeleteService eventGridImageBlobDeleteService)
        {
            _eventGridValidationService = eventGridValidationService;
            _eventGridImageBlobDeleteService = eventGridImageBlobDeleteService;
        }

        /// <summary>
        /// Handle event grid events from an endpoint request.
        /// </summary>
        /// <param name="events">The events to handle.</param>
        /// <param name="token">The access token for the request.</param>
        /// <param name="logger">The logger for the event handler.</param>
        /// <returns>The event handle result.</returns>
        public async Task<IActionResult> Handle(EventGridEvent[] events, string token, ILogger logger)
        {
            EventGridEvent gridEvent = events.FirstOrDefault();

            if (gridEvent == null)
            {
                logger.LogWarning("No image blob delete events were sent.");
                return new BadRequestResult();
            }

            if (gridEvent.EventType == EventTypes.EventGridSubscriptionValidationEvent)
            {
                logger.LogInformation("Handling web hook validation.");

                try
                {
                    logger.LogInformation("Successfully validated web hook.");
                    return new OkObjectResult(_eventGridValidationService.Validate(gridEvent));
                }
                catch (Exception)
                {
                    logger.LogError("Failed to validate subscription.");
                    return new BadRequestResult();
                }
            }

            if (gridEvent.EventType == EventTypes.StorageBlobDeletedEvent)
            {
                logger.LogInformation("Handling image blob delete.");

                try
                {
                    if (await _eventGridImageBlobDeleteService.DeleteImagePost(gridEvent, token))
                    {
                        logger.LogInformation("Successfully deleted image post record.");
                    }
                    else
                    {
                        logger.LogWarning("No image posts referencing the deleted blob exist.");
                    }

                    return new NoContentResult();
                }
                catch (InvalidTokenException)
                {
                    logger.LogError("Invalid image blob delete event token.");
                    return new UnauthorizedResult();
                }
            }

            logger.LogWarning("Unable to handle event.");

            return new BadRequestResult();
        }
    }
}