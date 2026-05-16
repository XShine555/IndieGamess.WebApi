namespace WebApi.DataTransferObjects.Users.Responses
{
    public record GetBasicUserResponse(
        Guid UserId,
        string DisplayName,
        UserProfilePictureResponse ProfilePicture,
        string Role);
}
