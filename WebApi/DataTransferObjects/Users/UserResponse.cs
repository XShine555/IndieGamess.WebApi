using WebApi.DataTransferObjects.Games;

namespace WebApi.DataTransferObjects.Users
{
    public record UserResponse(
        Guid Id,
        string Username,
        UserProfilePictureResponse ProfilePicture,
        IReadOnlyList<GameSummary> CreatedGames,
        IReadOnlyList<GameSummary> OwnedGames);
}
