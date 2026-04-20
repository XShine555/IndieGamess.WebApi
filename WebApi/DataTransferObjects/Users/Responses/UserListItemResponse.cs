namespace WebApi.DataTransferObjects.Users.Responses
{
    public record UserListItemResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        UserProfilePictureResponse ProfilePicture,
        int CreatedGamesCount,
        int OwnedGamesCount);
}
