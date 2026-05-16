using Application.Abstractions.Common;
using Application.Games.Catalog.Responses;
using WebApi.DataTransferObjects.AdminGame.Responses;

namespace WebApi.Common
{
    public interface IAdminGameApplicationMapper
    {
        PaginatedResponse<GameListItemAdminResponse> MapToGamePaginatedResponse(PaginatedApplicationResponse<ApplicationGameListItem> listItem);

        GameListItemAdminResponse MapToGameListItem(ApplicationGameListItem gameListItem);

        GameMutationAdminResponse MapToGameMutationResponse(ApplicationGameMutation applicationGame);

        GameGenresMutationAdminResponse MapToGameGenresMutationResponse(ApplicationGameGenresMutation applicationGame);

        Task<GameAdminResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken);
    }
}
