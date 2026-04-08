namespace WebApi.Contracts.Games
{
    public record CreateGameRequest(
        string Title,
        string Description,
        ICollection<int> Genres);
}