using Application.Users.Responses;

namespace WebApi.Contracts.Users
{
    public record UserSummary(
        string IdentityId,
        string Username)
    {
        public static UserSummary FromApplicationSummaryResponse(ApplicationUserSummary user)
        {
            return new UserSummary(user.IdentityId, user.Username);
        }
    }
}