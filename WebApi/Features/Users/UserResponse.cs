using Application.Users.Responses;

namespace WebApi.Features.Users;

public record UserResponse(
    string IdentityId,
    string Username,
    string DisplayUsername,
    UserPictureResponse? ProfilePicture,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static UserResponse FromApplicationResponse(ApplicationUser user)
    {
        return new UserResponse(
            user.IdentityId,
            user.Username,
            user.DisplayUsername,
            user.ProfilePicture is null
                ? null
                : UserPictureResponse.FromApplicationResponse(user.ProfilePicture),
            user.CreatedAt,
            user.UpdatedAt);
    }
}
