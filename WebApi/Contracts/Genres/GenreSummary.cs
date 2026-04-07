using Application.Genres.Responses;

namespace WebApi.Contracts.Genres
{
    public record GenreSummary(
        Guid Id,
        string Name)
    {
        public static GenreSummary FromApplicationResponse(ApplicationGenre genre)
        {
            return new GenreSummary(
                genre.Id,
                genre.Name);
        }
    }
}