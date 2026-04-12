using Application.Abstractions.Storage;
using Application.Games.Responses;

namespace WebApi.Features.Games
{
    public record GameStorePictureResponse(
        string SmallImageUrl,
        string MediumImageUrl,
        string LargeImageUrl)
    {
        public static async Task<GameStorePictureResponse> FromApplicationResponseAsync(ApplicationGamePicture gameStorePicture, IS3Service s3Service,
            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.SmallPictureKey, nameof(gameStorePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.MediumPictureKey, nameof(gameStorePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.LargePictureKey, nameof(gameStorePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new GameStorePictureResponse(smallImageUrl, mediumImageUrl, largeImageUrl);
        }
    }
}