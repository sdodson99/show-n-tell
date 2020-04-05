using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for getting user profile information.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Get the profile for a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user's entire profile. Null if the profile does not exist.</returns>
        Task<User> GetProfile(string username);

        /// <summary>
        /// Get the image posts for a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The list of image posts for the user's profile. Empty list of the profile does not exist.</returns>
        Task<IEnumerable<ImagePost>> GetImagePosts(string username);
    }
}
