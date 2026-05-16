namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameListItemResponse(
        Guid Id,
        string Title,
        decimal Price,
        decimal Discount,
        IReadOnlyCollection<GameArtworkSummary> Artworks);
}
