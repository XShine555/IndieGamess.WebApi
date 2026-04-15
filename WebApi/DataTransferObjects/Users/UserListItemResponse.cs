namespace WebApi.DataTransferObjects.Users
{
    public record UserListItemResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        UserProfilePictureResponse ProfilePicture,
        int CreatedGamesCount,
        int OwnedGamesCount);
}
