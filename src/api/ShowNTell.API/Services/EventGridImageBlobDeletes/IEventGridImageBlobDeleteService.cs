using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using ShowNTell.Domain.Exceptions;

namespace ShowNTell.API.Services.EventGridImageBlobDeletes
{
    public interface IEventGridImageBlobDeleteService
    {
        /// <summary>
        /// Delete an image post corresponding to the blob deleted in the grid event.
        /// </summary>
        /// <param name="gridEvent">The grid event reporting the deleted blob.</param>
        /// <param name="token">The authentication token for the blob delete.</param>
        /// <returns>True/false for success.</returns>
        /// <exception cref="InvalidTokenException">Thrown if token is invalid.</exception>
        Task<bool> DeleteImagePost(EventGridEvent gridEvent, string token);
    }
}