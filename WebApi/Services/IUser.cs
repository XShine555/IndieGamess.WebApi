namespace WebApi.Services
{
    public interface IUser
    {
        string IdentityId { get; }

        string UserName { get; }
    }
}