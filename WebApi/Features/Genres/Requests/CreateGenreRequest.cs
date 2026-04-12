using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Genres.Requests
{
    public record CreateGenreRequest(
        [Required] [MinLength(3)]
        string Name);
}
