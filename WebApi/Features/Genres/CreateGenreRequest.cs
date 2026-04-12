using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Genres;

public record CreateGenreRequest(
    [Required] [MinLength(3)]
    string Name);
