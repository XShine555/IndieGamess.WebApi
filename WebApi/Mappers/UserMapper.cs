using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Responses;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Games;
using WebApi.DataTransferObjects.Users;

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

        public async Task<Result<UserResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken)
        {
            return await result.MapAsync(source => MapToUserResponse(source, cancellationToken));
        }

        public async Task<UserResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            var profilePicture = await MapToUserProfilePictureResponse(applicationUser, cancellationToken);

            return new UserResponse(
                applicationUser.IdentityId,
                applicationUser.Username,
                profilePicture);
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
                gameCollection.CreatedAt,
                gameCollection.UpdatedAt);
        }

        public GameCollectionDetailsResponse MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection)
        {
            return new GameCollectionDetailsResponse(
                gameCollection.Id,
                gameCollection.Name,
                gameCollection.Games.Select(MapToGameListItemResponse).ToList(),
                gameCollection.CreatedAt,
                gameCollection.UpdatedAt);
        }

        GameListItemResponse MapToGameListItemResponse(ApplicationGame game)
        {
            return new GameListItemResponse(
                game.Id,
                game.Title,
                game.Price,
                game.Discount);
        }

        async Task<UserProfilePictureResponse> MapToUserProfilePictureResponse(ApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

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
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.SmallPictureKey, nameof(applicationUser.ProfilePicture.SmallPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.MediumPictureKey, nameof(applicationUser.ProfilePicture.MediumPictureKey));
            ArgumentException.ThrowIfNullOrEmpty(applicationUser.ProfilePicture?.LargePictureKey, nameof(applicationUser.ProfilePicture.LargePictureKey));

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
