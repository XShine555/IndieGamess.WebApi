using Application.Genres.Responses;

namespace WebApi.Features.Genres.Responses
{
    public record GenreSummaryResponse(
        int Id,
        string Name)
    {
        public static GenreSummaryResponse FromApplicationResponse(ApplicationGenre genre)
        {
            return new GenreSummaryResponse(
                genre.Id,
                genre.Name);
        }
    }
}
