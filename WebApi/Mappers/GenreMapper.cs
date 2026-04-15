using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Genres;

namespace WebApi.Mappers
{
    public class GenreMapper
    {
        public PaginatedResponse<GenreResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenre> paginatedResponse)
        {
            return PaginatedResponse<GenreResponse>.FromApplicationResponse(paginatedResponse, MapToGenreResponse);
        }

        public GenreResponse MapToGenreResponse(ApplicationGenre applicationGenre)
        {
            return new GenreResponse(applicationGenre.Id, applicationGenre.Name);
        }
    }
}