namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserListItemResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        UserProfilePictureResponse ProfilePicture,
        int CreatedGamesCount,
        int OwnedGamesCount,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
