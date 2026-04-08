namespace WebApi.Contracts.Games
{
    public record UpdateGameRequest(
        string? Title,
        string? Description,
        string? OwnerId,
        ICollection<Guid> Genres);
}