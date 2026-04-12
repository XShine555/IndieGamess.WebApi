using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Users
{
    public record UpdateUserRequest(
        [Required] [MinLength(3)]
        string DisplayUsername);
}
