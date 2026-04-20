namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record UpdateUserAdminResponse(
        Guid Id,
        string Username,
        string DisplayUsername,
        DateTime UpdatedAt);
}
