namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserAdminResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        UserProfilePictureAdminResponse ProfilePicture,
        int CreatedGamesCount,
        int OwnedGamesCount,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
