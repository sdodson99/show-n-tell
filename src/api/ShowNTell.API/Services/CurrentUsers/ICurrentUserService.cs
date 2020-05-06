using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Services.CurrentUsers
{
    public interface ICurrentUserService
    {
        User GetCurrentUser(ClaimsPrincipal claims);
        User GetCurrentUser(HttpContext context);
    }
}