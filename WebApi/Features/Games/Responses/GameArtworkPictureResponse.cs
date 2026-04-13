using Application.Abstractions.Storage;
using Application.Games.Responses;

namespace WebApi.Features.Games.Responses
{
    public record GameArtworkPictureResponse(
        string Type,
        string SmallUrl,
        string MediumUrl,
        string LargeUrl)
    {
        public static async Task<GameArtworkPictureResponse> FromApplicationResponseAsync(ApplicationGameArtwork gameArtwork, IS3Service s3Service,
    CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(gameArtwork.SmallArtworkKey, nameof(gameArtwork.SmallArtworkKey));
            ArgumentException.ThrowIfNullOrEmpty(gameArtwork.MediumArtworkKey, nameof(gameArtwork.MediumArtworkKey));
            ArgumentException.ThrowIfNullOrEmpty(gameArtwork.LargeArtworkKey, nameof(gameArtwork.LargeArtworkKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(gameArtwork.SmallArtworkKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(gameArtwork.MediumArtworkKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(gameArtwork.LargeArtworkKey, TimeSpan.FromHours(1), cancellationToken);
            return new GameArtworkPictureResponse(gameArtwork.Type.ToString(), smallImageUrl, mediumImageUrl, largeImageUrl);
        }
    }
}