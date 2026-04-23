using Application.Abstractions.Common;
using Application.Genres.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGenre.Responses;

namespace WebApi.Mappers
{
    public class AdminGenreApplicationMapper : IAdminGenreApplicationMapper
    {
        public PaginatedResponse<GenreListItemAdminResponse> MapToGenrePaginatedResponse(PaginatedApplicationResponse<ApplicationGenreListItem> paginatedResponse)
        {
            return PaginatedResponse<GenreListItemAdminResponse>.FromApplicationResponse(paginatedResponse, MapToGenreListItemResponse);
        }

        public GenreListItemAdminResponse MapToGenreListItemResponse(ApplicationGenreListItem applicationGenre)
        {
            return new GenreListItemAdminResponse(
                    applicationGenre.Id,
                    applicationGenre.Name,
                    applicationGenre.CreatedAt,
                    applicationGenre.UpdatedAt);
        }

        public GenreAdminResponse MapToGenreResponse(ApplicationGenre applicationGenre)
        {
            return new GenreAdminResponse(
                    applicationGenre.Id,
                    applicationGenre.Name,
                    applicationGenre.CreatedAt,
                    applicationGenre.UpdatedAt);
        }

        public GenreAdminSummary MapToGenreSummary(ApplicationGenreMutation applicationGenreMutation)
        {
            return new GenreAdminSummary(
                    applicationGenreMutation.Id,
                    applicationGenreMutation.Name);
        }
    }
}
