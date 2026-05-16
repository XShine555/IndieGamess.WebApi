using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.DataTransferObjects.AdminUser.Responses
{
    public record GameCollectionDetailsAdminResponse(
        Guid Id,
        string Name,
        IReadOnlyList<GameListItemResponse> Games,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
