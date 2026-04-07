using Application.Genres.Responses;

namespace WebApi.Contracts.Genres
{
    public record GenreResponse(
        Guid Id,
        string Name)
    {
        public static GenreResponse FromApplicationResponse(ApplicationGenre applicationGenre)
        {
            return new GenreResponse(
                applicationGenre.Id,
                applicationGenre.Name);
        }
    }
}