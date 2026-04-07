using Application.Users.Responses;

namespace WebApi.Contracts.Users
{
    public record UserSummary(
        Guid Id)
    {
        public static UserSummary FromApplicationResponse(ApplicationUser user)
        {
            return new UserSummary(user.Id);
        }
    }
}