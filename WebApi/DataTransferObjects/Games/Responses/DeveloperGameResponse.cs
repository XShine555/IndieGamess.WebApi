using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.DataTransferObjects.Games.Responses
{
    public record DeveloperGameResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        DeveloperGameReleaseBuildSummary? ReleaseBuild,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<DeveloperGameArtworkSummary> Artworks,
        IReadOnlyList<DeveloperGameStorePictureSummary> StorePictures,
        string Status);
}
