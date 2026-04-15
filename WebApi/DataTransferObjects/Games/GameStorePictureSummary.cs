namespace WebApi.DataTransferObjects.Games
{
    public record GameStorePictureSummary(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}