namespace WebApi.DataTransferObjects.AdminUser
{
    public record UpdateUserAdminResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        DateTime UpdatedAt);
}
