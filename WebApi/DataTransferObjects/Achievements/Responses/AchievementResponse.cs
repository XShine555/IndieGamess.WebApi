namespace WebApi.DataTransferObjects.Achievements.Responses
{
    public record AchievementResponse(
        Guid Id,
        Guid GameId,
        string Name,
        string Description,
        bool IsUnlocked,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
