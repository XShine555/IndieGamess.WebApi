using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.DataTransferObjects.Users.Responses
{
    public record GetUserLibraryResponse(
        IReadOnlyList<GameSummary> Games);
}
