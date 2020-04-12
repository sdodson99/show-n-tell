using Microsoft.AspNetCore.Http;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Services.CurrentUsers
{
    public interface ICurrentUserService
    {
        User GetCurrentUser(HttpContext context);
    }
}