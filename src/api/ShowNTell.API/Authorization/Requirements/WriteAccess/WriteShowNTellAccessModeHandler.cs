using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ShowNTell.API.Authorization.Requirements.WriteAccess
{
    public class WriteShowNTellAccessModeHandler : AuthorizationHandler<WriteAccessRequirement>
    {
        private readonly bool _writeAccessModeEnabled;

        public WriteShowNTellAccessModeHandler(bool writeAccessModeEnabled)
        {
            _writeAccessModeEnabled = writeAccessModeEnabled;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WriteAccessRequirement requirement)
        {
            if(_writeAccessModeEnabled)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}