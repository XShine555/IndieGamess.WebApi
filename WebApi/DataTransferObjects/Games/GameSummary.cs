using WebApi.DataTransferObjects.Genres;

namespace WebApi.DataTransferObjects.Games
{
    public record GameSummary(
        Guid Id,
        string Title,
        string Description,
        IReadOnlyCollection<GenreSummary> Genres,
        IReadOnlyCollection<GameStorePictureSummary> StorePictures,
        IReadOnlyCollection<GameArtworkSummary> Artworks);
}
