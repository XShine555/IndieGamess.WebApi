namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record UserProfilePictureAdminResponse(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}
