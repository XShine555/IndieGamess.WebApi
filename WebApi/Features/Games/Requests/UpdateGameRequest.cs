using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Games.Requests
{
    public record UpdateGameRequest(
        [Required] [MinLength(3)]
        string? Title,
        [Required] [MinLength(10)]
        string? Description,
        [Required]
        string? OwnerId,
        [Required]
        int[] Genres,
        [Required]
        bool IsPublic);
}
