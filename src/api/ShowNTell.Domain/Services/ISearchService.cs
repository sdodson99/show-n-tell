using System.Collections.Generic;
using System.Threading.Tasks;
using ShowNTell.Domain.Models;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service to search for image posts.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Search for image posts with a search query.
        /// </summary>
        /// <param name="query">The query to search for.</param>
        /// <returns>The list of image posts matching the search query.</returns>
         Task<IEnumerable<ImagePost>> SearchImagePosts(string query);
    }
}