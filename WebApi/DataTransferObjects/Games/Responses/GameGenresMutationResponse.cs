namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameGenresMutationResponse(
        Guid GameId,
        IReadOnlyList<Guid> GenreIds);
}
