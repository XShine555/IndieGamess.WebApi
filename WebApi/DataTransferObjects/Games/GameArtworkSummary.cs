namespace WebApi.DataTransferObjects.Games
{
    public record GameArtworkSummary(
        string SmallPictureUrl,
        string MediumPictureUrl,
        string LargePictureUrl);
}