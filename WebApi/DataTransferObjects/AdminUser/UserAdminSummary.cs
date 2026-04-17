namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserAdminSummary(
        Guid Id,
        string Username,
        string DisplayUsername);
}
