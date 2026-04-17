using WebApi.DataTransferObjects.AdminUser;
using WebApi.DataTransferObjects.Genres;

namespace WebApi.DataTransferObjects.AdminGame
{
    public record GameListItemAdminResponse(
        Guid Id,
        string Title,
        decimal Price,
        decimal Discount,
        bool IsReadyForStore,
        bool IsPublic,
        bool IsPublished,
        UserAdminSummary Owner,
        IReadOnlyList<GenreSummary> Genres,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
