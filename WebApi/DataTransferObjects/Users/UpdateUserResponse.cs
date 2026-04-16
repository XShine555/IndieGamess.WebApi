namespace WebApi.DataTransferObjects.Users
{
    public record UpdateUserResponse(
        Guid Id,
        string DisplayUsername);
}
