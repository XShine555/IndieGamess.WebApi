using Domain.Entities;

namespace WebApi.DataTransferObjects.AdminGame
{
    public record GameArtworkAdminSummary(
        Guid ArtworkId,
        GameArtworkType Type,
        string OriginalArtworkKey,
        string SmallArtworkKey,
        string SmallArtworkUrl,
        string MediumArtworkKey,
        string MediumArtworkUrl,
        string LargeArtworkKey,
        string LargeArtworkUrl,
        GameArtworkProcessingStatus ProcessingStatus,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
