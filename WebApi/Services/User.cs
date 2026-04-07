using System.Security.Claims;

namespace WebApi.Services
{
    public class User(IHttpContextAccessor httpContextAccessor)
        : IUser
    {
        ClaimsPrincipal ClaimsPrincipal => httpContextAccessor.HttpContext!.User;

        public Guid IdentityId => Guid.Parse(ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public string UserName => ClaimsPrincipal.FindFirstValue(ClaimTypes.Name)!;
    }
}