using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameSummary(
        Guid Id,
        string Title,
        string Description,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameStorePictureSummary> StorePictures,
        IReadOnlyList<GameArtworkSummary> Artworks);
}
