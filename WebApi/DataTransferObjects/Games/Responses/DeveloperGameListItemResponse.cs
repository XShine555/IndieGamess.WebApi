namespace WebApi.DataTransferObjects.Games.Responses
{
    public record DeveloperGameListItemResponse(
        Guid Id,
        string Title,
        string Description,
        string Status,
        bool IsPublic,
        IReadOnlyCollection<GameArtworkSummary> Artworks);
}
