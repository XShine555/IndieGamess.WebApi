using Application.Genres.Responses;

namespace WebApi.Features.Genres;

public record GenreResponse(
    int Id,
    string Name)
{
    public static GenreResponse FromApplicationResponse(ApplicationGenre applicationGenre)
    {
        return new GenreResponse(
            applicationGenre.Id,
            applicationGenre.Name);
    }
}
