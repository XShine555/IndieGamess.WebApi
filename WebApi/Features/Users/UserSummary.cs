using Application.Users.Responses;

namespace WebApi.Features.Users
{
    public record UserSummary(
        string IdentityId,
        string Username)
    {
        public static UserSummary FromApplicationSummaryResponse(ApplicationUserSummary user)
        {
            return new UserSummary(user.IdentityId, user.Username);
        }

        public static UserSummary FromApplicationResponse(ApplicationUser user)
        {
            return new UserSummary(user.IdentityId, user.Username);
        }
    }
}
