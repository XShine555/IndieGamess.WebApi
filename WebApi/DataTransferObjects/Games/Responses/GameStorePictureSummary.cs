namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameStorePictureSummary(
        string SmallImageUrl,
        string MediumImageUrl,
        string LargeImageUrl);
}