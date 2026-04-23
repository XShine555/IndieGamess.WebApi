using Application.Abstractions.Common;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGame.Responses;

namespace WebApi.Mappers
{
    public interface IAdminGameMapper
    {
        PaginatedResponse<GameListItemAdminResponse> MapToGamePaginatedResponse(PaginatedApplicationResponse<ApplicationGameListItem> listItem);

        GameListItemAdminResponse MapToGameListItem(ApplicationGameListItem gameListItem);

        GameMutationAdminResponse MapToGameMutationResponse(ApplicationGameMutation applicationGame);

        GameGenresMutationAdminResponse MapToGameGenresMutationResponse(ApplicationGameGenresMutation applicationGame);

        Task<GameAdminResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken);
    }
}
