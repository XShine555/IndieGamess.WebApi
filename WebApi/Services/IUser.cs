namespace WebApi.Services
{
    public interface IUser
    {
        Guid IdentityId { get; }

        string UserName { get; }
    }
}