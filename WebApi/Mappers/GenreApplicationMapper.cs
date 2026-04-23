using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Genres.Responses;

namespace WebApi.Mappers
{
    public class GenreApplicationMapper : IGenreApplicationMapper
    {
        public PaginatedResponse<GenreListItemResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse)
        {
            return PaginatedResponse<GenreListItemResponse>.FromApplicationResponse(paginatedResponse, MapToGenreListItemResponse);
        }

        public GenreListItemResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre)
        {
            return new GenreListItemResponse(applicationGenre.Id, applicationGenre.Name);
        }

        public GenreResponse MapToGenreResponse(ApplicationGenre applicationGenre)
        {
            return new GenreResponse(applicationGenre.Id, applicationGenre.Name);
        }
    }
}