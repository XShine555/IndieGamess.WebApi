using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Genres
{
    public record CreateGenreRequest(
        [Required] [MinLength(3)]
        string Name);
}
