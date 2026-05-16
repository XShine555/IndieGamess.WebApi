using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.DataTransferObjects.Users.Requests
{
    public record GetUserResponse(
        Guid Id,
        string Username,
        UserProfilePictureResponse ProfilePicture,
        IReadOnlyList<GameSummary> CreatedGames,
        IReadOnlyList<GameSummary> OwnedGames);
}
