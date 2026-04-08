using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Games
{
    public record UpdateGameRequest(
        [Required] [MinLength(3)]
        string? Title,
        [Required] [MinLength(10)]
        string? Description,
        [Required]
        string? OwnerId,
        [Required]
        int[] Genres);
}