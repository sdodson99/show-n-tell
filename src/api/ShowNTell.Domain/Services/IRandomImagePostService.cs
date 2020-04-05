using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service to retrieve a random image post.
    /// </summary>
    public interface IRandomImagePostService
    {
        /// <summary>
        /// Get a random image post.
        /// </summary>
        /// <returns>The random image post. Null if no image posts are available.</returns>
        Task<ImagePost> GetRandom();
    }
}
