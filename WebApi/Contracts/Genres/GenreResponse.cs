using Application.Genres.Responses;

namespace WebApi.Contracts.Genres
{
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
}