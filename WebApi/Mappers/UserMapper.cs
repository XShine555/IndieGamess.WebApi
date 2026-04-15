using Application.Abstractions.Common;
using Application.Abstractions.Storage;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Users;

namespace WebApi.Mappers
{
    public class UserMapper(IS3Service s3Service)
    {
        public async Task<PaginatedResponse<UserResponse>> MapToUserPaginatedResponseAsync(PaginatedApplicationResponse<ApplicationUser> paginatedResponse, CancellationToken cancellationToken)
        {
            return await PaginatedResponse<UserResponse>.FromApplicationResponseAsync(paginatedResponse,
                source => MapToUserResponse(source, cancellationToken), cancellationToken);
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

        public GameCollectionResponse MapToGameCollectionResponse(ApplicationUserCollection gameCollection)
        {
            return new GameCollectionResponse(
                gameCollection.Id,
                gameCollection.Name);
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
    }
}
