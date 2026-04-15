namespace WebApi.DataTransferObjects.Users
{
    public record UserResponse(
        Guid Id,
        string Username,
        UserProfilePictureResponse ProfilePicture);
}
