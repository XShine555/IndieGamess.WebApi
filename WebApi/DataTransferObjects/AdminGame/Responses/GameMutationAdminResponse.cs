namespace WebApi.DataTransferObjects.AdminGame.Responses
{
    public record GameMutationAdminResponse(
        Guid Id,
        Guid OwnerId,
        string Title,
        decimal Price,
        decimal Discount,
        bool IsPublic,
        bool IsPublished,
        DateTime UpdatedAt);
}
