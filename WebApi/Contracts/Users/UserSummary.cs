using Application.Users.Responses;

namespace WebApi.Contracts.Users
{
    public record UserSummary(
        string Id)
    {
        public static UserSummary FromApplicationResponse(ApplicationUser user)
        {
            return new UserSummary(user.IdentityId);
        }
    }
}