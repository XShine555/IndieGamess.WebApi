namespace WebApi.DataTransferObjects.Users
{
    public record GameCollectionListItemResponse(
        Guid Id,
        string Name,
        int GamesCount,
        IReadOnlyList<string> PreviewSmallPictureUrls,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
