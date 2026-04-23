using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.DataTransferObjects.AdminGenre.Responses;

namespace WebApi.Common
{
    public interface IAdminGenreApplicationMapper
    {
        PaginatedResponse<GenreListItemAdminResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse);

        GenreListItemAdminResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre);

        GenreAdminResponse MapToGenreResponse(ApplicationGenre applicationGenre);

        GenreAdminSummary MapToGenreSummary(ApplicationGenreMutation applicationGenreMutation);
    }
}
