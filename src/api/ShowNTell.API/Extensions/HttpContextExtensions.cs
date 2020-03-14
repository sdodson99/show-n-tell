using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShowNTell.API.Extensions
{
    public static class HttpContextExtensions
    {
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
