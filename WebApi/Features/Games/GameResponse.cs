using WebApi.Features.Genres;
using WebApi.Features.Users;

namespace WebApi.Features.Games
{
    public record GameResponse(
        int Id,
        string Title,
        string Description,
        UserSummary Owner,
        IReadOnlyCollection<GenreSummary> Genres,
        IReadOnlyCollection<GameStorePictureResponse> GameStorePictures);
}
