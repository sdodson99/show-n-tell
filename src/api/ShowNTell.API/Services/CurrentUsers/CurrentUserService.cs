using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Services.CurrentUsers
{
    public class CurrentUserService : ICurrentUserService
    {
        public User GetCurrentUser(HttpContext context)
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