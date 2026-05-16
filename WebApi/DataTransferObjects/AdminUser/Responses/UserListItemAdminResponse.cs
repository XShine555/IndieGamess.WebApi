namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record UserListItemAdminResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        UserProfilePictureAdminResponse ProfilePicture,
        int CreatedGamesCount,
        int OwnedGamesCount,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
