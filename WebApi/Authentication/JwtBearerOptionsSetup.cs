using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WebApi.Authentication
{
    public class JwtBearerOptionsSetup(IOptions<AuthenticationConfiguration> option)
        : IPostConfigureOptions<JwtBearerOptions>
    {
        AuthenticationConfiguration authenticationConfiguration => option.Value;

        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.Authority = authenticationConfiguration.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = authenticationConfiguration.Authority,
                RoleClaimType = ClaimTypes.Role,
            };
            options.EventsType = typeof(JwtBearerEventsHandler);
        }
    }
}