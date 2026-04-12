using Application.Users.Responses;

namespace WebApi.Features.Users.Responses
{
    public record UserResponse(
        string IdentityId,
        string Username,
        string DisplayUsername,
        UserPictureResponse? ProfilePicture,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
