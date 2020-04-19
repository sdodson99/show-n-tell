using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Services.EventGridImageBlobDeletes
{
    public class EventGridImageBlobDeleteService : IEventGridImageBlobDeleteService
    {
        private readonly IImagePostService _imagePostService;
        private readonly string _correctToken;

        public EventGridImageBlobDeleteService(IImagePostService imagePostService, string correctToken)
        {
            _imagePostService = imagePostService;
            _correctToken = correctToken;
        }

        /// <summary>
        /// Delete an image post corresponding to the blob deleted in the grid event.
        /// </summary>
        /// <param name="gridEvent">The grid event reporting the deleted blob.</param>
        /// <param name="token">The authentication token for the blob delete.</param>
        /// <returns>True/false for success.</returns>
        /// <exception cref="InvalidTokenException">Thrown if token is invalid.</exception>
        public async Task<bool> DeleteImagePost(EventGridEvent gridEvent, string token)
        {
            if(token != _correctToken)
            {
                throw new InvalidTokenException(token);
            }

            StorageBlobDeletedEventData eventData = JsonConvert.DeserializeObject<StorageBlobDeletedEventData>(gridEvent.Data.ToString());

            return await _imagePostService.DeleteByUri(eventData.Url);
        }
    }
}