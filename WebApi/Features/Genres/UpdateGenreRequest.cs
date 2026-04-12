using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Genres
{
    public record UpdateGenreRequest(
        [Required] [MinLength(3)]
        string? Name);
}
