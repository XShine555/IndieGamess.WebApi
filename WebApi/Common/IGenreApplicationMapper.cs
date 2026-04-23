using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.Common
{
    public interface IGenreApplicationMapper
    {
        PaginatedResponse<GenreListItemResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse);

        GenreListItemResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre);

        GenreResponse MapToGenreResponse(ApplicationGenre applicationGenre);
    }
}
