using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.Mappers
{
    public class GameMapper(IS3Service s3Service) : SignedUrlMapper(s3Service), IGameMapper
    {
        public async Task<PaginatedResponse<GameListItemResponse>> MapToGameListPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationGameListItem> listItem,
            CancellationToken cancellationToken)
        {
            return await PaginatedResponse<GameListItemResponse>.FromApplicationResponseAsync(
                listItem,
                item => MapToGameListItem(item, cancellationToken),
                cancellationToken);
        }

        public async Task<PaginatedResponse<GameResponse>> MapToGamePaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationGame> listItem,
            CancellationToken cancellationToken)
        {
            return await PaginatedResponse<GameResponse>.FromApplicationResponseAsync(
                listItem,
                item => MapToGameResponse(item, cancellationToken),
                cancellationToken);
        }

        public async Task<GameListItemResponse> MapToGameListItem(ApplicationGameListItem gameListItem, CancellationToken cancellationToken)
        {
            return new GameListItemResponse(
                gameListItem.Id,
                gameListItem.Title,
                gameListItem.Price,
                gameListItem.Discount,
                await MapArtworkSummary(gameListItem.Artworks, cancellationToken));
        }

        public GameMutationResponse MapToGameMutationResponse(ApplicationGameMutation applicationGame)
        {
            return new GameMutationResponse(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Price,
                applicationGame.Discount,
                applicationGame.IsPublic,
                applicationGame.IsPublished,
                applicationGame.OwnerId);
        }

        public GameGenresMutationResponse MapToGameGenresMutationResponse(ApplicationGameGenresMutation applicationGame)
        {
            return new GameGenresMutationResponse(
                applicationGame.GameId,
                applicationGame.GenreIds.ToList());
        }

        public async Task<GameResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken)
        {
            var artworks = await MapArtworkSummary(applicationGame.Artworks, cancellationToken);
            var storePictures = await MapStorePictureSummary(applicationGame.Pictures, cancellationToken);

            return new GameResponse(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Description,
                applicationGame.Price,
                applicationGame.Discount,
                MapUserSummary(applicationGame),
                MapGenreSummary(applicationGame),
                artworks,
                storePictures);
        }

        UserSummary MapUserSummary(ApplicationGame applicationGame)
        {
            return new UserSummary(applicationGame.Owner.IdentityId, applicationGame.Owner.Username);
        }

        IReadOnlyList<GenreSummary> MapGenreSummary(ApplicationGame applicationGame)
        {
            return applicationGame.Genres.Select(g => new GenreSummary(g.Id, g.Name)).ToList();
        }

        async Task<IReadOnlyList<GameArtworkSummary>> MapArtworkSummary(IReadOnlyCollection<ApplicationGameArtwork> applicationGameArtwork, CancellationToken cancellationToken)
        {
            var result = new List<GameArtworkSummary>(applicationGameArtwork.Count);
            foreach (var artwork in applicationGameArtwork)
            {
                var urls = await CreateSignedUrlsAsync(
                    artwork.SmallArtworkKey,
                    artwork.MediumArtworkKey,
                    artwork.LargeArtworkKey,
                    cancellationToken);

                result.Add(new GameArtworkSummary(
                    urls.SmallUrl,
                    urls.MediumUrl,
                    urls.LargeUrl));
            }

            return result;
        }

        async Task<IReadOnlyList<GameStorePictureSummary>> MapStorePictureSummary(IReadOnlyCollection<ApplicationGamePicture> storePictures, CancellationToken cancellationToken)
        {
            var result = new List<GameStorePictureSummary>(storePictures.Count);
            foreach (var storePicture in storePictures)
            {
                var urls = await CreateSignedUrlsAsync(
                    storePicture.SmallPictureKey,
                    storePicture.MediumPictureKey,
                    storePicture.LargePictureKey,
                    cancellationToken);

                result.Add(new GameStorePictureSummary(
                    urls.SmallUrl,
                    urls.MediumUrl,
                    urls.LargeUrl));
            }

            return result;
        }
    }
}