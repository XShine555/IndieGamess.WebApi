using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Users.Requests
{
    public record UpdateUserRequest(
        [Required] [MinLength(3)]
        string DisplayUsername);
}
