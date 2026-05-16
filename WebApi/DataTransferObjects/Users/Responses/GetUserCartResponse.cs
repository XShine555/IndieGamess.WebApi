using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.DataTransferObjects.Users.Responses
{
    public record GetUserCartResponse(
        IReadOnlyCollection<GameSummary> Game);
}
