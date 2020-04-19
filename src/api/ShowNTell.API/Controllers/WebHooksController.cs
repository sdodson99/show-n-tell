using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShowNTell.API.Models;
using ShowNTell.API.Services.EventGridImageBlobDeletes;
using ShowNTell.API.Services.EventGridValidations;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("hooks")]
    public class WebHooksController : ControllerBase
    {
        private readonly IEventGridImageBlobDeleteService _eventGridImageBlobDeleteService;
        private readonly IEventGridValidationService _eventGridValidationService;
        private readonly ILogger<WebHooksController> _logger;

        public WebHooksController(IEventGridImageBlobDeleteService eventGridImageBlobDeleteService, IEventGridValidationService eventGridValidationService, ILogger<WebHooksController> logger)
        {
            _eventGridImageBlobDeleteService = eventGridImageBlobDeleteService;
            _eventGridValidationService = eventGridValidationService;
            _logger = logger;
        }


        /// <summary>
        /// Handle an image blob delete event from Azure Storage.
        /// </summary>
        /// <param name="events">The events to handle.</param>
        /// <param name="token">The authentication token for the delete event.</param>
        [HttpPost("handle-image-blob-delete")]
        public async Task<IActionResult> HandleImageBlobDelete([FromBody] EventGridEvent[] events, [FromQuery] string token)
        {
            _logger.LogInformation("Received image blob delete event.");

            EventGridEvent gridEvent = events.FirstOrDefault();

            if(gridEvent == null)
            {
                _logger.LogWarning("No image blob delete events were sent.");
                return BadRequest();
            }

            if(gridEvent.EventType == EventTypes.EventGridSubscriptionValidationEvent)
            {
                _logger.LogInformation("Handling web hook validation.");
                
                try
                {
                    _logger.LogInformation("Successfully validated web hook.");
                    return Ok(_eventGridValidationService.Validate(gridEvent));
                }
                catch (System.Exception)
                {
                    _logger.LogError("Failed to validate subscription.");
                    return BadRequest();
                }
            }

            if(gridEvent.EventType == EventTypes.StorageBlobDeletedEvent)
            {
                _logger.LogInformation("Handling image blob delete.");

                try
                {
                    if(await _eventGridImageBlobDeleteService.DeleteImagePost(gridEvent, token))
                    {
                        _logger.LogInformation("Successfully deleted image post record.");
                        return NoContent();
                    }
                    
                    _logger.LogError("Failed to delete image post record.");
                    return NotFound();
                }
                catch (InvalidTokenException)
                {
                    _logger.LogError("Invalid image blob delete event token.");
                    return Unauthorized();
                }
            }

            _logger.LogWarning("Unable to handle event.");
            return BadRequest();
        }   
    }
}