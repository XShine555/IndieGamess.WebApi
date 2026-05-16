namespace WebApi.DataTransferObjects.Achievements.Responses
{
    public record AchievementDeveloperResponse(
        Guid Id,
        Guid GameId,
        string Name,
        string Description,
        string PictureState,
        bool IsPublished,
        string? SmallPictureUrl,
        string? MediumPictureUrl,
        string? LargePictureUrl);
}
