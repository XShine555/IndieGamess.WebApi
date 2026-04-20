namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameArtworkSummary(
        string SmallImageUrl,
        string MediumImageUrl,
        string LargeImageUrl);
}