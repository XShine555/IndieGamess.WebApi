namespace WebApi.DataTransferObjects.Achievements.Responses
{
    public record AchievementMutationResponse(
        Guid Id,
        Guid GameId,
        string Name,
        string Description,
        DateTime CreatedAt);
}
