using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.DataTransferObjects.Users.Responses
{
    public record GameCollectionDetailsResponse(
        Guid Id,
        string Name,
        IReadOnlyList<GameListItemResponse> Games,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
