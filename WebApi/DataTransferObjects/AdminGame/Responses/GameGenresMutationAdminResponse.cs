namespace WebApi.DataTransferObjects.AdminGame.Responses
{
    public record GameGenresMutationAdminResponse(
        Guid GameId,
        IReadOnlyList<Guid> GenreIds);
}
