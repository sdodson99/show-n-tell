using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShowNTell.API.Extensions
{
    /// <summary>
    /// Extension methods for the HttpContext class.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Get a user from the HttpContext.
        /// </summary>
        /// <param name="context">The HttpContext to use.</param>
        /// <returns>The user from the HttpContext claims.</returns>
        public static User GetUser(this HttpContext context)
        {
            string email = context.User.FindFirst(ClaimTypes.Email).Value;
            string username = email.Substring(0, email.IndexOf('@'));

            return new User()
            {
                Email = email,
                Username = username
            };
        }
    }
}
