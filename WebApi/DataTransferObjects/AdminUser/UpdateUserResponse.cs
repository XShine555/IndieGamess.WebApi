namespace WebApi.DataTransferObjects.AdminUser
{
    public record UpdateUserResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        DateTime UpdatedAt);
}
