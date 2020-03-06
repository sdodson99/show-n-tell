using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using ShowNTell.Prototype.Utilities;

namespace ShowNTell.Prototype.Extensions
{
    public static class AddGoogleJWTAuthenticationExtension
    {
        public static void AddGoogleJWTAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new GoogleTokenValidator());
            });
        }
    }
}
