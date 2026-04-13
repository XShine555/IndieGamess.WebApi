using WebApi.Features.Genres.Responses;
using WebApi.Features.Users.Responses;

namespace WebApi.Features.Games.Responses
{
    public record GameResponse(
        int Id,
        string Title,
        string Description,
        UserSummaryResponse Owner,
        IReadOnlyCollection<GenreSummaryResponse> Genres,
        IReadOnlyCollection<GameArtworkPictureResponse> GameArtworkPictures,
        IReadOnlyCollection<GameStorePictureResponse> GameStorePictures);
}
