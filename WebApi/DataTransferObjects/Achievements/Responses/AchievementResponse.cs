namespace WebApi.DataTransferObjects.Achievements.Responses
{
    public record AchievementResponse(
        Guid Id,
        Guid GameId,
        string Name,
        string Description,
        string? SmallPictureUrl,
        string? MediumPictureUrl,
        string? LargePictureUrl,
        bool? IsUnlocked,
        DateTime? UnlockedAt);
}
