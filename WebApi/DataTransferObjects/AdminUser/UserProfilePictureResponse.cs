namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserProfilePictureResponse(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}
