using System.Security.Claims;

namespace WebApi.Services
{
    public class User(IHttpContextAccessor httpContextAccessor)
        : IUser
    {
        ClaimsPrincipal ClaimsPrincipal => httpContextAccessor.HttpContext!.User;

        public string IdentityId => ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}