using WebApi.DataTransferObjects.Games;

namespace WebApi.DataTransferObjects.Users
{
    public record GetUserCartResponse(
        IReadOnlyCollection<GameSummary> Game);
}
