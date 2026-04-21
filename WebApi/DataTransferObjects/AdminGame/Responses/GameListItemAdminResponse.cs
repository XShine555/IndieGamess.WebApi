using WebApi.DataTransferObjects.AdminUser.Responses;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.DataTransferObjects.AdminGame.Responses
{
    public record GameListItemAdminResponse(
        Guid Id,
        string Title,
        decimal Price,
        decimal Discount,
        bool IsPublic,
        bool IsPublished,
        UserAdminSummary Owner,
        IReadOnlyList<GenreSummary> Genres,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
