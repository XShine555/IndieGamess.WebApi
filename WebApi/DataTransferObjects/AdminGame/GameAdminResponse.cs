using WebApi.DataTransferObjects.AdminUser;
using WebApi.DataTransferObjects.Genres;

namespace WebApi.DataTransferObjects.AdminGame
{
    public record GameAdminResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        decimal Discount,
        bool IsReadyForStore,
        bool IsPublic,
        bool IsPublished,
        UserAdminSummary Owner,
        IReadOnlyList<GenreSummary> Genres,
        IReadOnlyList<GameArtworkAdminSummary> Artworks,
        IReadOnlyList<GameStorePictureAdminSummary> StorePictures,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
