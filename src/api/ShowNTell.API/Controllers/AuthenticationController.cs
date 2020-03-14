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
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("auth/google")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GoogleLogin()
        {
            User currentUser = HttpContext.GetUser();

            currentUser = await _userService.GetByEmail(currentUser.Email);
            
            // User does not exist.
            if(currentUser == null)
            {
                string username = currentUser.Email.Substring(0, currentUser.Email.IndexOf('@'));
                currentUser.Username = username;
                currentUser.DateJoined = DateTime.Now;

                currentUser = await _userService.Create(currentUser);
            }

            return Ok(currentUser);
        }
    }
}