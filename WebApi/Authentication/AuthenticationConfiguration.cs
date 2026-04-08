using System.ComponentModel.DataAnnotations;

namespace WebApi.Authentication
{
    public class AuthenticationConfiguration
    {
        public const string SectionName = "Authentication";

        [Required]
        public required string Region { get; set; }

        [Required]
        public required string Authority { get; set; }

        [Required]
        public required string UserPoolId { get; set; }

        [Required]
        public required string ClientId { get; set; }

        [Required]
        public required string AuthorizationEndpoint { get; set; }

        [Required]
        public required string TokenEndpoint { get; set; }
    }
}