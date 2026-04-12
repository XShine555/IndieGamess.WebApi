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
            var smallImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new GameStorePictureResponse(smallImageUrl, mediumImageUrl, largeImageUrl);
        }
    }
}