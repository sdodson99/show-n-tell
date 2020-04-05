using System.Collections.Generic;
using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service to get a feed of image posts for a user.
    /// </summary>
    public interface IFeedService
    {
        /// <summary>
        /// Get the feed of image posts for a user.
        /// </summary>
        /// <param name="userEmail">The email of the user to retrieve a feed for.</param>
        /// <returns>The collection of image posts representing the feed.</returns>
        /// <exception cref="System.Exception">Thrown if retrieving the feed fails.</exception>
         Task<IEnumerable<ImagePost>> GetFeed(string userEmail);
    }
}