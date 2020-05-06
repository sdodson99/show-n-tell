using Microsoft.AspNetCore.Authorization;
using ShowNTell.API.Authorization.Requirements.AdminOverride;

namespace ShowNTell.API.Authorization.Requirements.ReadAccess
{
    public class ReadAccessRequirement : IAuthorizationRequirement {}
}