using WebApi.DataTransferObjects.AdminUser.Responses;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.DataTransferObjects.AdminGame.Responses
{
    public record GameAdminResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        bool IsPublic,
        bool IsPublished,
        UserAdminSummary Owner,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameArtworkAdminSummary> Artworks,
        IReadOnlyList<GameStorePictureAdminSummary> StorePictures,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
