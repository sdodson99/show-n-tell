using Microsoft.AspNetCore.Authorization;
using ShowNTell.API.Authorization.Requirements.AdminOverride;

namespace ShowNTell.API.Authorization.Requirements.WriteAccess
{
    public class WriteAccessRequirement : IAuthorizationRequirement { }
}