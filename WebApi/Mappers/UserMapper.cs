using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Application.Genres.Responses;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.DataTransferObjects.Users.Requests;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.Mappers
{
    public class UserMapper(IS3Service s3Service)
    {
        public async Task<PaginatedResponse<UserListItemResponse>> MapToUserPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationUserListItem> paginatedResponse,
            CancellationToken cancellationToken)
        {
            return await PaginatedResponse<UserListItemResponse>.FromApplicationResponseAsync(
                paginatedResponse,
                source => MapToUserListItemResponse(source, cancellationToken),
                cancellationToken);
        }

        public PaginatedResponse<GameCollectionListItemResponse> MapToGameCollectionPaginatedResponse(
            PaginatedApplicationResponse<ApplicationUserCollectionListItem> paginatedResponse)
        {
            return PaginatedResponse<GameCollectionListItemResponse>.FromApplicationResponse(
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

        public async Task<Result<UserResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken)
        {
            return await result.MapAsync(source => MapToUserResponse(source, cancellationToken));
        }

        public async Task<UserResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            var profilePicture = await MapToUserProfilePictureResponse(applicationUser, cancellationToken);

            var createdGames = await Task.WhenAll(
                applicationUser.CreatedGames.Select(game => MapToGameSummaryResponse(game, cancellationToken))
            );

            var ownedGames = await Task.WhenAll(
                applicationUser.OwnedGames.Select(game => MapToGameSummaryResponse(game, cancellationToken))
            );

            return new UserResponse(
                applicationUser.IdentityId,
                applicationUser.Username,
                profilePicture,
                createdGames.ToList(),
                ownedGames.ToList());
        }

        public UpdateUserResponse MapToUpdateUserResponse(ApplicationUserMutation applicationUser)
        {
            return new UpdateUserResponse(
                    applicationUser.IdentityId,
                    applicationUser.DisplayUsername);
        }

        public async Task<UserListItemResponse> MapToUserListItemResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken)
        {
            var profilePicture = await MapToUserProfilePictureResponse(applicationUser, cancellationToken);

            return new UserListItemResponse(
                applicationUser.IdentityId,
                applicationUser.Username,
                applicationUser.DisplayUsername,
                profilePicture,
                applicationUser.CreatedGamesCount,
                applicationUser.OwnedGamesCount);
        }

        public GameCollectionResponse MapToGameCollectionResponse(ApplicationUserCollectionListItem gameCollection)
        {
            return new GameCollectionResponse(
                gameCollection.Id,
                gameCollection.Name);
        }

        public GameCollectionListItemResponse MapToGameCollectionListItemResponse(ApplicationUserCollectionListItem gameCollection)
        {
            return new GameCollectionListItemResponse(
                gameCollection.Id,
                gameCollection.Name,
                gameCollection.GamesCount,
                gameCollection.PreviewSmallPictureUrls,
                gameCollection.CreatedAt);
        }

        public async Task<GameCollectionDetailsResponse> MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection, CancellationToken cancellationToken)
        {
            var games = await Task.WhenAll(
                gameCollection.Games.Select(game => MapToGameListItemResponse(game, cancellationToken))
            );

            return new GameCollectionDetailsResponse(
                gameCollection.Id,
                gameCollection.Name,
                games,
                gameCollection.CreatedAt,
                gameCollection.UpdatedAt);
        }

        async Task<GameSummary> MapToGameSummaryResponse(ApplicationGame applicationGame, CancellationToken cancellationToken)
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

        async Task<GameListItemResponse> MapToGameListItemResponse(ApplicationGame game, CancellationToken cancellationToken)
        {
            var artworks = await Task.WhenAll(
                game.Artworks.Select(artwork => MapToGameArtworkSummaryResponse(artwork, cancellationToken))
            );

            return new GameListItemResponse(
                game.Id,
                game.Title,
                game.Price,
                game.Discount,
                artworks);
        }

        async Task<UserProfilePictureResponse> MapToUserProfilePictureResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new UserProfilePictureResponse(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }

        async Task<UserProfilePictureResponse> MapToUserProfilePictureResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

            var smallImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.SmallPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var mediumImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.MediumPictureKey, TimeSpan.FromHours(1), cancellationToken);
            var largeImageUrl = await s3Service.GetSignedUrlAsync(applicationUser.ProfilePicture.LargePictureKey, TimeSpan.FromHours(1), cancellationToken);
            return new UserProfilePictureResponse(
                smallImageUrl,
                mediumImageUrl,
                largeImageUrl);
        }
    }
}
