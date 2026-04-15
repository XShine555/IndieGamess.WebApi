namespace WebApi.DataTransferObjects.Users
{
    public record UserProfilePictureResponse(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}
