using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShowNTell.API.Models;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("hooks")]
    public class WebHooksController : ControllerBase
    {
        private readonly IImagePostService _imagePostService;
        private readonly WebHookTokenConfiguration _tokens;
        private readonly ILogger<WebHooksController> _logger;

        public WebHooksController(IImagePostService imagePostService, WebHookTokenConfiguration tokens,
            ILogger<WebHooksController> logger)
        {
            _imagePostService = imagePostService;
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

                string data = gridEvent.Data.ToString();
                SubscriptionValidationEventData eventData = JsonConvert.DeserializeObject<SubscriptionValidationEventData>(data);

                if(string.IsNullOrEmpty(eventData.ValidationCode))
                {
                    _logger.LogWarning("Failed to retrieve validation code.");
                    return BadRequest();
                }

                return Ok(new SubscriptionValidationResponse(eventData.ValidationCode));
            }

            _logger.LogWarning("Unable to handle event.");
            return BadRequest();
        }   
    }
}