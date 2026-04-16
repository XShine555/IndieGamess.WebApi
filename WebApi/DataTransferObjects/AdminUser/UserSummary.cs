namespace WebApi.DataTransferObjects.AdminUser
{
    public record UserSummary(
        Guid Id,
        string Username,
        string DisplayUsername);
}
