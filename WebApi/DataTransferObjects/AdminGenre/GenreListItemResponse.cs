namespace WebApi.DataTransferObjects.AdminGenre
{
    public record GenreListItemResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
