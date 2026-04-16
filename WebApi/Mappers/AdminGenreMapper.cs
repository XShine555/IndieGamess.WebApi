using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGenre;

namespace WebApi.Mappers
{
    public class AdminGenreMapper
    {
        public PaginatedResponse<GenreListItemResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse)
        {
            return PaginatedResponse<GenreListItemResponse>.FromApplicationResponse(paginatedResponse, MapToGenreListItemResponse);
        }

        public GenreListItemResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre)
        {
            return new GenreListItemResponse(
                    applicationGenre.Id,
                    applicationGenre.Name,
                    applicationGenre.CreatedAt,
                    applicationGenre.UpdatedAt);
        }

        public GenreResponse MapToGenreResponse(ApplicationGenre applicationGenre)
        {
            return new GenreResponse(
                    applicationGenre.Id,
                    applicationGenre.Name,
                    applicationGenre.CreatedAt,
                    applicationGenre.UpdatedAt);
        }

        public GenreSummary MapToGenreSummary(ApplicationGenreMutation applicationGenreMutation)
        {
            return new GenreSummary(
                    applicationGenreMutation.Id,
                    applicationGenreMutation.Name);
        }
    }
}
