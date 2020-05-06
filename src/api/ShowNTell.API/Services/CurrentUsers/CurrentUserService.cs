using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Services.CurrentUsers
{
    public class CurrentUserService : ICurrentUserService
    {
        public User GetCurrentUser(ClaimsPrincipal claims)
        {
            Claim emailClaim = claims.FindFirst(ClaimTypes.Email);

            if(emailClaim == null)
            {
                return null;
            }

            string email = emailClaim.Value;
            string username = email.Substring(0, email.IndexOf('@'));

            return new User()
            {
                Email = email,
                Username = username
            };
        }

        public User GetCurrentUser(HttpContext context)
        {
            return GetCurrentUser(context.User);
        }
    }
}