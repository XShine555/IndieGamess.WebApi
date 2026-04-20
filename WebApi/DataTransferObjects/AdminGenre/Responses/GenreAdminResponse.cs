namespace WebApi.DataTransferObjects.AdminGenre.Responses
{
    public record GenreAdminResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
