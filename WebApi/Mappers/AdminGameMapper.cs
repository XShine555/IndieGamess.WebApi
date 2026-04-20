using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Catalog.Responses;
using Application.Genres.Responses;
using Application.Users.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGame.Responses;
using WebApi.DataTransferObjects.AdminUser.Responses;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.Mappers
{
    public class AdminGameMapper(IS3Service s3Service)
    {
        public PaginatedResponse<GameListItemAdminResponse> MapToGamePaginatedResponse(PaginatedApplicationResponse<ApplicationGameListItem> listItem)
        {
            return PaginatedResponse<GameListItemAdminResponse>.FromApplicationResponse(listItem, MapToGameListItem);
        }

        public GameListItemAdminResponse MapToGameListItem(ApplicationGameListItem gameListItem)
        {
            return new GameListItemAdminResponse(
                gameListItem.Id,
                gameListItem.Title,
                gameListItem.Price,
                gameListItem.Discount,
                gameListItem.IsReadyForStore,
                gameListItem.IsPublic,
                gameListItem.IsPublished,
                MapOwner(gameListItem.Owner),
                gameListItem.Genres.Select(MapGenreSummary).ToList(),
                gameListItem.CreatedAt,
                gameListItem.UpdatedAt);
        }

        public GameMutationAdminResponse MapToGameMutationResponse(ApplicationGameMutation applicationGame)
        {
            return new GameMutationAdminResponse(
                applicationGame.Id,
                applicationGame.OwnerId,
                applicationGame.Title,
                applicationGame.Price,
                applicationGame.Discount,
                applicationGame.IsPublic,
                applicationGame.IsPublished,
                applicationGame.UpdatedAt);
        }

        public GameGenresMutationAdminResponse MapToGameGenresMutationResponse(ApplicationGameGenresMutation applicationGame)
        {
            return new GameGenresMutationAdminResponse(
                applicationGame.GameId,
                applicationGame.GenreIds.ToList());
        }

        public async Task<GameAdminResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var artworks = await MapArtworkSummary(applicationGame, cancellationToken);
            var storePictures = await MapStorePictureSummary(applicationGame, cancellationToken);

            return new GameAdminResponse(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Description,
                applicationGame.Price,
                applicationGame.Discount,
                applicationGame.IsReadyForStore,
                applicationGame.IsPublic,
                applicationGame.IsPublished,
                MapOwner(applicationGame.Owner),
                applicationGame.Genres.Select(MapGenreSummary).ToList(),
                artworks,
                storePictures,
                applicationGame.CreatedAt,
                applicationGame.UpdatedAt);
        }

        UserAdminSummary MapOwner(ApplicationUserMutation owner)
        {
            return new UserAdminSummary(owner.IdentityId, owner.Username, owner.DisplayUsername);
        }

        GenreSummary MapGenreSummary(ApplicationGenre genre)
        {
            return new GenreSummary(genre.Id, genre.Name);
        }

        async Task<IReadOnlyList<GameArtworkAdminSummary>> MapArtworkSummary(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var result = new List<GameArtworkAdminSummary>(applicationGame.Artworks.Count);
            foreach (var artwork in applicationGame.Artworks)
            {
                ArgumentException.ThrowIfNullOrEmpty(artwork.OriginalArtworkKey, nameof(artwork.OriginalArtworkKey));
                ArgumentException.ThrowIfNullOrEmpty(artwork.SmallArtworkKey, nameof(artwork.SmallArtworkKey));
                ArgumentException.ThrowIfNullOrEmpty(artwork.MediumArtworkKey, nameof(artwork.MediumArtworkKey));
                ArgumentException.ThrowIfNullOrEmpty(artwork.LargeArtworkKey, nameof(artwork.LargeArtworkKey));

                var smallImageUrl = await s3Service.GetSignedUrlAsync(artwork.SmallArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                var mediumImageUrl = await s3Service.GetSignedUrlAsync(artwork.MediumArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                var largeImageUrl = await s3Service.GetSignedUrlAsync(artwork.LargeArtworkKey, TimeSpan.FromHours(1), cancellationToken);
                result.Add(new GameArtworkAdminSummary(
                    artwork.ArtworkId,
                    artwork.Type,
                    artwork.OriginalArtworkKey,
                    artwork.SmallArtworkKey,
                    smallImageUrl,
                    artwork.MediumArtworkKey,
                    mediumImageUrl,
                    artwork.LargeArtworkKey,
                    largeImageUrl,
                    artwork.ProcessingStatus,
                    artwork.CreatedAt,
                    artwork.UpdatedAt));
            }

            return result;
        }

        async Task<IReadOnlyList<GameStorePictureAdminSummary>> MapStorePictureSummary(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var result = new List<GameStorePictureAdminSummary>(applicationGame.Pictures.Count);
            foreach (var storePicture in applicationGame.Pictures)
            {
                ArgumentException.ThrowIfNullOrEmpty(storePicture.OriginalPictureKey, nameof(storePicture.OriginalPictureKey));
                ArgumentException.ThrowIfNullOrEmpty(storePicture.SmallPictureKey, nameof(storePicture.SmallPictureKey));
                ArgumentException.ThrowIfNullOrEmpty(storePicture.MediumPictureKey, nameof(storePicture.MediumPictureKey));
                ArgumentException.ThrowIfNullOrEmpty(storePicture.LargePictureKey, nameof(storePicture.LargePictureKey));

                var smallImageUrl = await s3Service.GetSignedUrlAsync(storePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
                var mediumImageUrl = await s3Service.GetSignedUrlAsync(storePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
                var largeImageUrl = await s3Service.GetSignedUrlAsync(storePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
                result.Add(new GameStorePictureAdminSummary(
                    storePicture.PictureId,
                    storePicture.OriginalPictureKey,
                    storePicture.SmallPictureKey,
                    smallImageUrl,
                    storePicture.MediumPictureKey,
                    mediumImageUrl,
                    storePicture.LargePictureKey,
                    largeImageUrl,
                    storePicture.ProcessingStatus,
                    storePicture.AddedAt));
            }

            return result;
        }
    }
}
