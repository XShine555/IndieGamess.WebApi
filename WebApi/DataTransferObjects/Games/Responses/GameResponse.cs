using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        UserSummary Owner,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameArtworkSummary> Artworks,
        IReadOnlyList<GameStorePictureSummary> StorePictures);
}
