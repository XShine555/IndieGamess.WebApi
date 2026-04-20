namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record UserAdminSummary(
        Guid Id,
        string Username,
        string DisplayUsername);
}
