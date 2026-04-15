using System.Security.Claims;

namespace WebApi.Services
{
    public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor)
        : ICurrentUser
    {
        private ClaimsPrincipal ClaimsPrincipal =>
            httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("An active HTTP context is required.");

        public Guid IdentityId =>
            Guid.Parse(ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
