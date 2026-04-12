using Application.Abstractions.Storage;
using Application.Users.Responses;

namespace WebApi.Features.Users
{
    public record UserPictureResponse(
        int PictureId,
        string OriginalImageUrl,
        string SmallImageUrl,
        string MediumImageUrl,
        string LargeImageUrl,
        DateTime AddedAt)
    {
        public static async Task<UserPictureResponse> FromApplicationResponseAsync(
            ApplicationUserPicture picture,
            IS3Service s3Service,
            CancellationToken cancellationToken)
        {
            var originalImageUrl = await s3Service.GetSignedUrlAsync(picture.OriginalPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var smallImageUrl = await s3Service.GetSignedUrlAsync(picture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(picture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(picture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);

            return new UserPictureResponse(
                picture.PictureId,
                originalImageUrl,
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl,
                picture.AddedAt);
        }
    }
}
