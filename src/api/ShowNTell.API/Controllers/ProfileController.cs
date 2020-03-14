using Microsoft.AspNetCore.Mvc;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Controllers
{
    [ApiController]
    [Route("{username:string}")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        
    }
}