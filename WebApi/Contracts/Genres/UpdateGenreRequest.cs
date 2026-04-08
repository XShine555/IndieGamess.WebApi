using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Genres
{
    public record UpdateGenreRequest(
        [Required] [MinLength(3)]
        string? Name);
}
