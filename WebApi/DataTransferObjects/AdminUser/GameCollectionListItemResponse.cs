namespace WebApi.DataTransferObjects.AdminUser
{
    public record GameCollectionListItemResponse(
        Guid Id,
        string Name,
        int GamesCount,
        IReadOnlyList<string> PreviewSmallPictureUrls,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
