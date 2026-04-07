namespace WebApi.Contracts.Games
{
    public record UpdateGameRequest(
        string? Title,
        string? Description,
        Guid? OwnerId,
        ICollection<Guid> Genres);
}