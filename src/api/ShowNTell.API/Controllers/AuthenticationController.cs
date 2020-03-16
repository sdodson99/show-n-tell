using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ShowNTell.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using System;
using ShowNTell.API.Models.Responses;
using AutoMapper;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("google")]
        [Authorize]
        public async Task<IActionResult> GoogleLogin()
        {
            User currentUser = HttpContext.GetUser();

            User existingUser = await _userService.GetByEmail(currentUser.Email);
            
            // User does not exist.
            if(existingUser == null)
            {
                currentUser.DateJoined = DateTime.Now;
                existingUser = await _userService.Create(currentUser);
            }

            return Ok(_mapper.Map<UserResponse>(existingUser));
        }
    }
}