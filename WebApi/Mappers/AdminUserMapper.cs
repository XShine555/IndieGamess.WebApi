using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Games.Responses;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminUser;
using WebApi.DataTransferObjects.Games;

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

        public PaginatedResponse<GameCollectionListItemResponse> MapToGameCollectionPaginatedResponse(
            PaginatedApplicationResponse<ApplicationUserCollectionListItem> paginatedResponse)
        {
            return PaginatedResponse<GameCollectionListItemResponse>.FromApplicationResponse(
                paginatedResponse,
                MapToGameCollectionListItemResponse);
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

        public GameCollectionDetailsAdminResponse MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection)
        {
            return new GameCollectionDetailsAdminResponse(
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
