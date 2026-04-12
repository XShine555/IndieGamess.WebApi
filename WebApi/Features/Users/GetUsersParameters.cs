using System.ComponentModel;

namespace WebApi.Features.Users
{
    public record GetUsersParameters
    {
        [DefaultValue("")]
        public string Name { get; init; } = string.Empty;
    }
}
