namespace WebApi.Contracts.Games
{
    public record GameSummary(
        Guid Id,
        string Name,
        Guid OwnerId);
}