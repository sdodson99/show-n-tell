using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ShowNTell.API.Authorization.Requirements.ReadAccess
{
    public class ReadShowNTellAccessModeHandler : AuthorizationHandler<ReadAccessRequirement>
    {
        private readonly bool _readAccessModeEnabled;

        public ReadShowNTellAccessModeHandler(bool readAccessModeEnabled)
        {
            _readAccessModeEnabled = readAccessModeEnabled;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ReadAccessRequirement requirement)
        {
            if(_readAccessModeEnabled)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}