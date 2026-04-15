using WebApi.DataTransferObjects.Genres;

namespace WebApi.DataTransferObjects.Games
{
    public record GameResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameArtworkSummary> Artworks,
        IReadOnlyList<GameStorePictureSummary> StorePictures);
}
