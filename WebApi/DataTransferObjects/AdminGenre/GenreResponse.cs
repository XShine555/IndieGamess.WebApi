namespace WebApi.DataTransferObjects.AdminGenre
{
    public record GenreResponse(
        Guid Id,
        string Name,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
