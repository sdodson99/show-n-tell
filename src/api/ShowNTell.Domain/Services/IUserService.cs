using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.Domain.Services
{
    /// <summary>
    /// A service to manage users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get a user by email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user with the email. Null if the user does not exist.</returns>
        Task<User> GetByEmail(string email);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The new user to create.</param>
        /// <returns>The created user.</returns>
        /// <exception cref="System.Exception">Thrown if creating the new user fails.</exception>
        Task<User> Create(User user);
    }
}
