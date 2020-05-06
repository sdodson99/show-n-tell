using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ShowNTell.API.Services.CurrentUsers;
using ShowNTell.Domain.Models;

namespace ShowNTell.API.Authorization.Requirements.AdminOverride
{
    public class UsernameAdminOverrideHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        private readonly ICurrentUserService _userService;
        private readonly IEnumerable<string> _adminUsernames;

        public UsernameAdminOverrideHandler(ICurrentUserService userService, string adminUsername) : this(userService, new List<string>() { adminUsername }){}

        public UsernameAdminOverrideHandler(ICurrentUserService userService, IEnumerable<string> adminUsernames)
        {
            _userService = userService;
            _adminUsernames = adminUsernames;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            User currentUser = _userService.GetCurrentUser(context.User);

            if(currentUser != null && _adminUsernames.Any(u => u == currentUser.Username))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}