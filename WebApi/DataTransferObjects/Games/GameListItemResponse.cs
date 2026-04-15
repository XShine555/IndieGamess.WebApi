namespace WebApi.DataTransferObjects.Games
{
    public record GameListItemResponse(
        Guid Id,
        string Title,
        decimal Price,
        decimal Discount);
}
