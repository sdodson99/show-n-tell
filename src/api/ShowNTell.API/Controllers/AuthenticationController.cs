using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ShowNTell.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using System;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("google")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GoogleLogin()
        {
            User currentUser = HttpContext.GetUser();

            User existingUser = await _userService.GetByEmail(currentUser.Email);
            
            // User does not exist.
            if(existingUser == null)
            {
                string username = currentUser.Email.Substring(0, currentUser.Email.IndexOf('@'));
                currentUser.Username = username;
                currentUser.DateJoined = DateTime.Now;

                existingUser = await _userService.Create(currentUser);
            }

            return Ok(existingUser);
        }
    }
}