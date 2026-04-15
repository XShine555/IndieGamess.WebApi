namespace WebApi.DataTransferObjects.Games
{
    public record GameGenresMutationResponse(
        Guid GameId,
        IReadOnlyList<Guid> GenreIds);
}
