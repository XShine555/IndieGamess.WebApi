namespace WebApi.DataTransferObjects.Users.Responses
{
    public record GameCollectionListItemResponse(
        Guid Id,
        string Name,
        int GamesCount,
        IReadOnlyList<string> PreviewSmallPictureUrls,
        DateTime CreatedAt);
}
