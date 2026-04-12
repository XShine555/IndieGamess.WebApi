using Application.Abstractions.Storage;
using Application.Users.Responses;
using WebApi.Features.Users.Responses;

namespace WebApi.Features.Users
{
    public sealed class UserResponseMapper(IS3Service s3Service)
    {
        public async Task<UserResponse> FromApplicationResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var profilePicture = user.ProfilePicture is null
                ? null
                : await UserPictureResponse.FromApplicationResponseAsync(user.ProfilePicture, s3Service, cancellationToken);

            return new UserResponse(
                user.IdentityId,
                user.Username,
                user.DisplayUsername,
                profilePicture,
                user.CreatedAt,
                user.UpdatedAt);
        }
    }
}