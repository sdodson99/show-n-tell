using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShowNTell.API.Models;
using ShowNTell.API.Services.EventGridValidations;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("hooks")]
    public class WebHooksController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly EventGridValidationService _eventGridValidationService;
        private readonly WebHookTokenConfiguration _tokens;
        private readonly ILogger<WebHooksController> _logger;

        public WebHooksController(IImagePostService imagePostService, EventGridValidationService eventGridValidationService, 
            WebHookTokenConfiguration tokens, ILogger<WebHooksController> logger)
        {
            _imagePostService = imagePostService;
            _eventGridValidationService = eventGridValidationService;
            _tokens = tokens;
            _logger = logger;
        }

        /// <summary>
        /// Handle an image blob delete event from Azure Storage.
        /// </summary>
        /// <param name="events">The events to handle.</param>
        /// <param name="token">The authentication token for the delete event.</param>
        [HttpPost("handle-image-blob-delete")]
        public async Task<IActionResult> ImageBlobDelete([FromBody] EventGridEvent[] events, [FromQuery] string token)
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

                if(token != _tokens.ImageBlobDeleteToken)
                {
                    _logger.LogError("Invalid image blob delete event token {0} {1}.", token, _tokens.ImageBlobDeleteToken);
                    return Unauthorized();
                }

                StorageBlobDeletedEventData eventData = JsonConvert.DeserializeObject<StorageBlobDeletedEventData>(gridEvent.Data.ToString());
                _logger.LogInformation("URL of deleted image blob: {0}.", eventData.Url);

                bool success = await _imagePostService.DeleteByUri(eventData.Url);

                if(success)
                {
                    _logger.LogInformation("Successfully deleted image post record.");
                    return NoContent();
                }
                else
                {
                    _logger.LogError("Failed to delete image post record.");
                    return BadRequest();
                }
            }

            _logger.LogWarning("Unable to handle event.");
            return BadRequest();
        }   
    }
}