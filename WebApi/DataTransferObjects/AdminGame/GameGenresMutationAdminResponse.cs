namespace WebApi.DataTransferObjects.AdminGame
{
    public record GameGenresMutationAdminResponse(
        Guid GameId,
        IReadOnlyList<Guid> GenreIds);
}
