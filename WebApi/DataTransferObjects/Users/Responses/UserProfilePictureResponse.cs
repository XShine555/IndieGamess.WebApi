namespace WebApi.DataTransferObjects.Users.Responses
{
    public record UserProfilePictureResponse(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}
