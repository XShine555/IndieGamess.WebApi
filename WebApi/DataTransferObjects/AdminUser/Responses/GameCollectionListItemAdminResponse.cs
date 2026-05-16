namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record GameCollectionListItemAdminResponse(
        Guid Id,
        string Name,
        int GamesCount,
        IReadOnlyList<string> PreviewSmallPictureUrls,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
