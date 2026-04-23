using Application.Abstractions.Common;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.Mappers
{
    public interface IGameMapper
    {
        Task<PaginatedResponse<GameListItemResponse>> MapToGameListPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationGameListItem> listItem,
            CancellationToken cancellationToken);

        Task<PaginatedResponse<GameResponse>> MapToGamePaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationGame> listItem,
            CancellationToken cancellationToken);

        Task<GameListItemResponse> MapToGameListItem(ApplicationGameListItem gameListItem, CancellationToken cancellationToken);

        GameMutationResponse MapToGameMutationResponse(ApplicationGameMutation applicationGame);

        GameGenresMutationResponse MapToGameGenresMutationResponse(ApplicationGameGenresMutation applicationGame);

        Task<GameResponse> MapToGameResponse(ApplicationGame applicationGame, CancellationToken cancellationToken);
    }
}
