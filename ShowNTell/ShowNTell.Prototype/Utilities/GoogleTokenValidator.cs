using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShowNTell.Prototype.Utilities
{
    public class GoogleTokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            //Delegate validation to Google.
            validatedToken = null;
            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(securityToken).Result;

            //Get claims from validated Google JWT payload.
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, payload.Email)
            };

            //Return claims to caller as validated JWT principal.
            ClaimsPrincipal principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
            return principal;
        }
    }
}
