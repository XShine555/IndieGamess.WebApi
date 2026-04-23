using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGenre.Responses;

namespace WebApi.Mappers
{
    public interface IAdminGenreMapper
    {
        PaginatedResponse<GenreListItemAdminResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse);

        GenreListItemAdminResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre);

        GenreAdminResponse MapToGenreResponse(ApplicationGenre applicationGenre);

        GenreAdminSummary MapToGenreSummary(ApplicationGenreMutation applicationGenreMutation);
    }
}
