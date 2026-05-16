namespace WebApi.DataTransferObjects.Games.Responses
{
    public record DeveloperGameArtworkSummary(
        Guid Id,
        string? SmallPictureUrl,
        string? MediumPictureUrl,
        string? LargePictureUrl,
        string ProcessingStatus);
}
