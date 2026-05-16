namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameMutationResponse(
        Guid Id,
        string Title,
        decimal Price,
        decimal Discount,
        bool IsPublic,
        bool IsPublished,
        Guid OwnerId);
}
