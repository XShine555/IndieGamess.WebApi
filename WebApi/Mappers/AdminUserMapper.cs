using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Application.Genres.Responses;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Users.Responses;
using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.DataTransferObjects.AdminUser.Responses;

namespace WebApi.Mappers
{
    public class AdminUserMapper(IS3Service s3Service)
    {
        public async Task<PaginatedResponse<UserListItemAdminResponse>> MapToUserPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationUserListItem> paginatedResponse,
            CancellationToken cancellationToken)
        {
            return await PaginatedResponse<UserListItemAdminResponse>.FromApplicationResponseAsync(
                paginatedResponse,
                source => MapToUserListItemResponse(source, cancellationToken),
                cancellationToken);
        }

        public PaginatedResponse<GameCollectionListItemAdminResponse> MapToGameCollectionPaginatedResponse(
            PaginatedApplicationResponse<ApplicationUserCollectionListItem> paginatedResponse)
        {
            return PaginatedResponse<GameCollectionListItemAdminResponse>.FromApplicationResponse(
                paginatedResponse,
                MapToGameCollectionListItemResponse);
        }

        public async Task<Result<GetUserCartResponse>> MapToGetUserCartResponse(
            Result<IReadOnlyCollection<ApplicationUserCartItem>> result,
            CancellationToken cancellationToken)
        {
            return await result.MapAsync(source => MapToGetUserCartResponse(source, cancellationToken));
        }

        public async Task<GetUserCartResponse> MapToGetUserCartResponse(
            IReadOnlyCollection<ApplicationUserCartItem> cartItems,
            CancellationToken cancellationToken)
        {
            var mappedCartItems = await Task.WhenAll(
                cartItems.Select(cartItem => MapToGameSummaryResponse(cartItem.Game, cancellationToken)));

            return new GetUserCartResponse(mappedCartItems.ToList());
        }

        public async Task<Result<UserAdminResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken)
        {
            return await result.MapAsync(source => MapToUserResponse(source, cancellationToken));
        }

        public async Task<UserAdminResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            var profilePicture = await MapToUserProfilePictureResponse(applicationUser, cancellationToken);

            return new UserAdminResponse(
                applicationUser.IdentityId,
                applicationUser.Username,
                applicationUser.DisplayUsername,
                profilePicture,
                applicationUser.CreatedGames.Count,
                applicationUser.OwnedGames.Count,
                applicationUser.CreatedAt,
                applicationUser.UpdatedAt);
        }

        public UpdateUserAdminResponse MapToUpdateUserResponse(ApplicationUserMutation applicationUser)
        {
            return new UpdateUserAdminResponse(
                    applicationUser.IdentityId,
                    applicationUser.Username,
                    applicationUser.DisplayUsername,
                    applicationUser.UpdatedAt);
        }

        public async Task<UserListItemAdminResponse> MapToUserListItemResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken)
        {
            var profilePicture = await MapToUserProfilePictureResponse(applicationUser, cancellationToken);

            return new UserListItemAdminResponse(
                applicationUser.IdentityId,
                applicationUser.Username,
                applicationUser.DisplayUsername,
                profilePicture,
                applicationUser.CreatedGamesCount,
                applicationUser.OwnedGamesCount,
                applicationUser.CreatedAt,
                applicationUser.UpdatedAt);
        }

        public GameCollectionAdminResponse MapToGameCollectionResponse(ApplicationUserCollectionListItem gameCollection)
        {
            return new GameCollectionAdminResponse(
                gameCollection.Id,
                gameCollection.Name);
        }

        public GameCollectionListItemAdminResponse MapToGameCollectionListItemResponse(ApplicationUserCollectionListItem gameCollection)
        {
            return new GameCollectionListItemAdminResponse(
                gameCollection.Id,
                gameCollection.Name,
                gameCollection.GamesCount,
                gameCollection.PreviewSmallPictureUrls,
                gameCollection.CreatedAt,
                gameCollection.UpdatedAt);
        }

        public GameCollectionDetailsAdminResponse MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection)
        {
            return new GameCollectionDetailsAdminResponse(
                gameCollection.Id,
                gameCollection.Name,
                gameCollection.Games.Select(MapToGameListItemResponse).ToList(),
                gameCollection.CreatedAt,
                gameCollection.UpdatedAt);
        }

        async Task<GameSummary> MapToGameSummaryResponse(ApplicationUserGame applicationGame, CancellationToken cancellationToken)
        {
            var artworks = await Task.WhenAll(
                applicationGame.Artworks.Select(artwork => MapToGameArtworkSummaryResponse(artwork, cancellationToken))
            );

            var storePictures = await Task.WhenAll(
                applicationGame.Pictures.Select(picture => MapToGameStorePictureSummaryResponse(picture, cancellationToken))
            );

            return new GameSummary(
                applicationGame.Id,
                applicationGame.Title,
                applicationGame.Description,
                applicationGame.Genres.Select(MapToGenreSummaryResponse).ToList(),
                storePictures.ToList(),
                artworks.ToList());
        }

        GenreSummary MapToGenreSummaryResponse(ApplicationGenre applicationGenre)
        {
            return new GenreSummary(
                applicationGenre.Id,
                applicationGenre.Name);
        }

        async Task<GameArtworkSummary> MapToGameArtworkSummaryResponse(ApplicationGameArtwork applicationGameArtwork, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationGameArtwork.SmallArtworkKey, nameof(applicationGameArtwork.LargeArtworkKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationGameArtwork.MediumArtworkKey, nameof(applicationGameArtwork.LargeArtworkKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationGameArtwork.LargeArtworkKey, nameof(applicationGameArtwork.LargeArtworkKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(applicationGameArtwork.SmallArtworkKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(applicationGameArtwork.MediumArtworkKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(applicationGameArtwork.LargeArtworkKey, TimeSpan.FromHours(1), cancellationToken);

            return new GameArtworkSummary(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }

        async Task<GameStorePictureSummary> MapToGameStorePictureSummaryResponse(ApplicationGamePicture gameStorePicture, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.SmallPictureKey, nameof(gameStorePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.MediumPictureKey, nameof(gameStorePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(gameStorePicture.LargePictureKey, nameof(gameStorePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(gameStorePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);

            return new GameStorePictureSummary(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }

        GameListItemResponse MapToGameListItemResponse(ApplicationUserGame game)
        {
            return new GameListItemResponse(
                game.Id,
                game.Title,
                0,
                0);
        }

        GameListItemResponse MapToGameListItemResponse(ApplicationGame game)
        {
            return new GameListItemResponse(
                game.Id,
                game.Title,
                game.Price,
                game.Discount);
        }

        async Task<UserProfilePictureAdminResponse> MapToUserProfilePictureResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new UserProfilePictureAdminResponse(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }

        async Task<UserProfilePictureAdminResponse> MapToUserProfilePictureResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new UserProfilePictureAdminResponse(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }
    }
}
