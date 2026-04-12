using Application.Genres.Responses;

namespace WebApi.Features.Genres
{
    public record GenreSummary(
        int Id,
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
