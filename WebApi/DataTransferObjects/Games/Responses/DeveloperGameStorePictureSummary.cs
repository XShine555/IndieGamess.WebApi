namespace WebApi.DataTransferObjects.Games.Responses
{
    public record DeveloperGameStorePictureSummary(
        Guid Id,
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl,
        string ProcessingStatus);
}
