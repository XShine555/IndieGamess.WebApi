using Application.Abstractions.Storage;

namespace WebApi.Mappers
{
    public abstract class SignedUrlMapper(IS3Service s3Service)
    {
        protected readonly record struct PictureSignedUrls(string SmallUrl, string MediumUrl, string LargeUrl);

        protected Task<string> CreateSignedUrlAsync(string key, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
            return s3Service.GetSignedUrlAsync(key, TimeSpan.FromHours(1), cancellationToken);
        }

        protected async Task<PictureSignedUrls> CreatePictureSignedUrlsAsync(
            string smallKey,
            string mediumKey,
            string largeKey,
            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(smallKey, nameof(smallKey));
            ArgumentException.ThrowIfNullOrEmpty(mediumKey, nameof(mediumKey));
            ArgumentException.ThrowIfNullOrEmpty(largeKey, nameof(largeKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(smallKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(mediumKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(largeKey, TimeSpan.FromHours(1), cancellationToken);

            return new PictureSignedUrls(smallImageUrl, mediumImageUrl, largeImageUrl);
        }
    }
}
