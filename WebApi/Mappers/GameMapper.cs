using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Games;
using WebApi.DataTransferObjects.Genres;

namespace WebApi.Mappers
{
    public class GameMapper(IS3Service s3Service)
    {
        public async Task<PaginatedResponse<GameResponse>> MapToGamePaginatedResponse(PaginatedApplicationResponse<ApplicationGame> paginatedResponse,
            CancellationToken cancellationToken)
        {
            return await PaginatedResponse<GameResponse>.FromApplicationResponseAsync(paginatedResponse,
                source => MapToGameResponse(source, cancellationToken),
                cancellationToken);
        }

        public async Task<GameResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var artworks = await MapArtworkSummary(applicationGame, cancellationToken);
            var storePictures = await MapStorePictureSummary(applicationGame, cancellationToken);

            return new GameResponse(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Description,
                applicationGame.Price,
                applicationGame.Discount,
                MapGenreSummary(applicationGame),
                artworks,
                storePictures
            );
        }

        IReadOnlyList<GenreSummary> MapGenreSummary(ApplicationGame applicationGame)
        {
            return applicationGame.Genres.Select(g => new GenreSummary(g.Id, g.Name)).ToList();
        }

        async Task<IReadOnlyList<GameArtworkSummary>> MapArtworkSummary(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var result = new List<GameArtworkSummary>(applicationGame.Artworks.Count);
            foreach (var artwork in applicationGame.Artworks)
            {
                ArgumentException.ThrowIfNullOrEmpty(artwork.SmallArtworkKey, nameof(artwork.SmallArtworkKey));
                ArgumentException.ThrowIfNullOrEmpty(artwork.MediumArtworkKey, nameof(artwork.MediumArtworkKey));
                ArgumentException.ThrowIfNullOrEmpty(artwork.LargeArtworkKey, nameof(artwork.LargeArtworkKey));

                var smallImageUrl = await s3Service.GetSignedUrlAsync(artwork.SmallArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                var mediumImageUrl = await s3Service.GetSignedUrlAsync(artwork.MediumArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                var largeImageUrl = await s3Service.GetSignedUrlAsync(artwork.LargeArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                result.Add(new GameArtworkSummary(
                    smallImageUrl,
                    mediumImageUrl,
                    largeImageUrl));
            }
            return result;
        }

        async Task<IReadOnlyList<GameStorePictureSummary>> MapStorePictureSummary(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var result = new List<GameStorePictureSummary>(applicationGame.Pictures.Count);
            foreach (var storePicture in applicationGame.Pictures)
            {
                ArgumentException.ThrowIfNullOrEmpty(storePicture.SmallPictureKey, nameof(storePicture.SmallPictureKey));
                ArgumentException.ThrowIfNullOrEmpty(storePicture.MediumPictureKey, nameof(storePicture.MediumPictureKey));
                ArgumentException.ThrowIfNullOrEmpty(storePicture.LargePictureKey, nameof(storePicture.LargePictureKey));

                var smallImageUrl = await s3Service.GetSignedUrlAsync(storePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
                var mediumImageUrl = await s3Service.GetSignedUrlAsync(storePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
                var largeImageUrl = await s3Service.GetSignedUrlAsync(storePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
                result.Add(new GameStorePictureSummary(
                    smallImageUrl,
                    mediumImageUrl,
                    largeImageUrl));
            }
            return result;
        }
    }
}