using WebApi.DataTransferObjects.Games;

namespace WebApi.DataTransferObjects.AdminUser
{
    public record GameCollectionDetailsAdminResponse(
        Guid Id,
        string Name,
        IReadOnlyList<GameListItemResponse> Games,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
