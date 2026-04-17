namespace WebApi.DataTransferObjects.AdminGenre
{
    public record GenreListItemAdminResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
