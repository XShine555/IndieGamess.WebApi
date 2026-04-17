namespace WebApi.DataTransferObjects.AdminGenre
{
    public record GenreAdminResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
