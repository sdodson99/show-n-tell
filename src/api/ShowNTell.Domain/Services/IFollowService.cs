using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service for following and unfollowing users.
    /// </summary>
    public interface IFollowService
    {
        /// <summary>
        /// Follow a user.
        /// </summary>
        /// <param name="username">The username of the user to follow.</param>
        /// <param name="followerEmail">The email of the user requesting the follow.</param>
        /// <returns>The new follow.</returns>
        /// <exception cref="ShowNTell.Domain.Exceptions.OwnProfileFollowException">Thrown if user is attempting to follow their own profile.</exception>
        /// <exception cref="ShowNTell.Domain.Exceptions.EntityNotFoundException<string>">Thrown if the user to follow does not exist.</exception>
        /// <exception cref="System.Exception">Thrown if creating the follow fails.</exception>
        Task<Follow> FollowUser(string username, string followerEmail);

        /// <summary>
        /// Unfollow a user.
        /// </summary>
        /// <param name="username">The username of the user to unfollow.</param>
        /// <param name="followerEmail">The email of the user requesting the unfollow.</param>
        /// <returns>True/false for success.</returns>
        Task<bool> UnfollowUser(string username, string followerEmail);
    }
}
