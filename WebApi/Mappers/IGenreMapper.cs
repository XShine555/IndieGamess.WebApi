using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.Mappers
{
    public interface IGenreMapper
    {
        PaginatedResponse<GenreListItemResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse);

        GenreListItemResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre);

        GenreResponse MapToGenreResponse(ApplicationGenre applicationGenre);
    }
}
