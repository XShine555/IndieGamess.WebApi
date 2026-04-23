using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Application.Genres.Responses;
using Application.Users.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGame.Responses;
using WebApi.DataTransferObjects.AdminUser.Responses;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.Mappers
{
    public class AdminGameMapper(IS3Service s3Service) : SignedUrlMapper(s3Service), IAdminGameMapper
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
            var artworks = await MapArtworkSummary(applicationGame.Artworks, cancellationToken);
            var storePictures = await MapStorePictureSummary(applicationGame.Pictures, cancellationToken);

            return new GameAdminResponse(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Description,
                applicationGame.Price,
                applicationGame.Discount,
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

        async Task<IReadOnlyList<GameArtworkAdminSummary>> MapArtworkSummary(IReadOnlyCollection<ApplicationGameArtwork> artworks, CancellationToken cancellationToken)
        {
            var result = new List<GameArtworkAdminSummary>(artworks.Count);
            foreach (var artwork in artworks)
            {
                var urls = await CreateSignedUrlsAsync(
                    artwork.SmallArtworkKey,
                    artwork.MediumArtworkKey,
                    artwork.LargeArtworkKey,
                    cancellationToken);

                result.Add(new GameArtworkAdminSummary(
                    artwork.ArtworkId,
                    artwork.Type,
                    artwork.OriginalArtworkKey,
                    artwork.SmallArtworkKey,
                    urls.SmallUrl,
                    artwork.MediumArtworkKey,
                    urls.MediumUrl,
                    artwork.LargeArtworkKey,
                    urls.LargeUrl,
                    artwork.ProcessingStatus,
                    artwork.CreatedAt,
                    artwork.UpdatedAt));
            }

            return result;
        }

        async Task<IReadOnlyList<GameStorePictureAdminSummary>> MapStorePictureSummary(IReadOnlyCollection<ApplicationGamePicture> storePictures, CancellationToken cancellationToken)
        {
            var result = new List<GameStorePictureAdminSummary>(storePictures.Count);
            foreach (var storePicture in storePictures)
            {
                var urls = await CreateSignedUrlsAsync(
                    storePicture.SmallPictureKey,
                    storePicture.MediumPictureKey,
                    storePicture.LargePictureKey,
                    cancellationToken);

                result.Add(new GameStorePictureAdminSummary(
                    storePicture.PictureId,
                    storePicture.OriginalPictureKey,
                    storePicture.SmallPictureKey,
                    urls.SmallUrl,
                    storePicture.MediumPictureKey,
                    urls.MediumUrl,
                    storePicture.LargePictureKey,
                    urls.LargeUrl,
                    storePicture.ProcessingStatus,
                    storePicture.AddedAt));
            }

            return result;
        }
    }
}
