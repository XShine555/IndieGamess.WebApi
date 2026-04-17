namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserProfilePictureAdminResponse(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}
