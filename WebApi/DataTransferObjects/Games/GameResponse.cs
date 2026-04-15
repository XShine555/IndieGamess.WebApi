using WebApi.DataTransferObjects.Genres;
using WebApi.DataTransferObjects.Users;

namespace WebApi.DataTransferObjects.Games
{
    public record GameResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        UserSummary User,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameArtworkSummary> Artworks,
        IReadOnlyList<GameStorePictureSummary> StorePictures);
}
