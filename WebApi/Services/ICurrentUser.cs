namespace WebApi.Services
{
    public interface ICurrentUser
    {
        Guid IdentityId { get; }
    }
}
