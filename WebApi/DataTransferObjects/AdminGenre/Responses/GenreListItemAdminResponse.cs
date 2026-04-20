namespace WebApi.DataTransferObjects.AdminGenre.Responses
{
    public record GenreListItemAdminResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
