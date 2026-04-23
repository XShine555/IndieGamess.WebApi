using Application.Abstractions.Storage;

namespace WebApi.Mappers
{
    public abstract class SignedUrlMapper(IS3Service s3Service)
    {
        protected readonly record struct SignedUrls(string SmallUrl, string MediumUrl, string LargeUrl);

        protected async Task<SignedUrls> CreateSignedUrlsAsync(
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

            return new SignedUrls(smallImageUrl, mediumImageUrl, largeImageUrl);
        }
    }
}
