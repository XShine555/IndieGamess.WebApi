using System.Security.Claims;

namespace WebApi.Services
{
    public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor)
        : ICurrentUser
    {
        private ClaimsPrincipal ClaimsPrincipal =>
            httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException("An active HTTP context is required.");

        public string IdentityId =>
            ClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("The authenticated user does not contain a NameIdentifier claim.");
    }
}
