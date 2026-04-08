using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using WebApi.Authentication;

namespace WebApi.Scalar
{
    public class ScalarOptionsSetup(IOptions<AuthenticationConfiguration> option)
        : IConfigureOptions<ScalarOptions>
    {
        AuthenticationConfiguration authenticationConfiguration => option.Value;

        public void Configure(ScalarOptions options)
        {
            options.AddAuthorizationCodeFlow("OAuth2", authorizationCodeFlow =>
            {
                authorizationCodeFlow.ClientId = authenticationConfiguration.ClientId;
                authorizationCodeFlow.AuthorizationUrl = authenticationConfiguration.AuthorizationEndpoint;
                authorizationCodeFlow.TokenUrl = authenticationConfiguration.TokenEndpoint;
                authorizationCodeFlow.Pkce = Pkce.Sha256;
            });
        }
    }
}
