using Application.Users.Responses;

namespace WebApi.Features.Users.Responses
{
    public record UserSummaryResponse(
        string IdentityId,
        string Username)
    {
        public static UserSummaryResponse FromApplicationSummaryResponse(ApplicationUserSummary user)
        {
            return new UserSummaryResponse(user.IdentityId, user.Username);
        }

        public static UserSummaryResponse FromApplicationResponse(ApplicationUser user)
        {
            return new UserSummaryResponse(user.IdentityId, user.Username);
        }
    }
}
