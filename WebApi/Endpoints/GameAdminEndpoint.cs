using Application.Games.Catalog.Commands;
using Application.Games.Catalog.Queries;
using Application.Games.Media.Commands;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGame.Requests;
using WebApi.DataTransferObjects.AdminGame.Responses;
using WebApi.Extensions;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("admin/game")]
    [Tags("Admin Game")]
    [Authorize]
    public class GameAdminEndpoint(IMediator mediator, IAdminGameMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Game Admins")]
        [EndpointSummary("Get Games")]
        public async Task<PaginatedResponse<GameListItemAdminResponse>> Get([FromQuery] GetGamesAdminRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGamePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get Game By Id Admin")]
        [EndpointSummary("Get Game By Id")]
        public async Task<GameAdminResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Game Admin")]
        [EndpointSummary("Create Game")]
        public async Task<GameMutationAdminResponse> CreateGame([FromForm] CreateGameAdminRequest createGameRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var capsulePicture = FileData.FromFormFile(createGameRequest.CapsulePicture);
            var headerPicture = FileData.FromFormFile(createGameRequest.HeaderPicture);
            var mainPicture = FileData.FromFormFile(createGameRequest.MainPicture);
            var commandResult = await mediator.Send(new CreateGameCommand(
                currentUser.IdentityId,
                createGameRequest.Title,
                createGameRequest.Description,
                createGameRequest.Price,
                createGameRequest.Genres,
                capsulePicture,
                headerPicture,
                mainPicture), cancellationToken);
            return commandResult.Map(r => mapper.MapToGameMutationResponse(r));
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}", Name = "Remove Game Admin")]
        [EndpointSummary("Remove Game")]
        public async Task<Result> RemoveGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update Game Admin")]
        [EndpointSummary("Update Game")]
        public async Task<GameMutationAdminResponse> UpdateGame(Guid id, [FromBody] UpdateGameAdminRequest updateGameRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameCommand(
                currentUser.IdentityId,
                id,
                updateGameRequest.Title,
                updateGameRequest.Description,
                updateGameRequest.Price,
                updateGameRequest.Discount,
                updateGameRequest.IsPublic), cancellationToken);
            return commandResult.Map(r => mapper.MapToGameMutationResponse(r));
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/publish", Name = "Publish Game Admin")]
        [EndpointSummary("Publish Game")]
        public async Task<Result> PublishGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PublishGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/genres", Name = "Update Game Genre Admins")]
        [EndpointSummary("Update Game Genres")]
        public async Task<GameGenresMutationAdminResponse> UpdateGenres(Guid id, [FromBody] UpdateGameGenresAdminRequest updateGameGenresRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameGenresCommand(currentUser.IdentityId, id, updateGameGenresRequest.Genres), cancellationToken);
            return commandResult.Map(r => mapper.MapToGameGenresMutationResponse(r));
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/owner", Name = "Change Game Owner Admin")]
        [EndpointSummary("Change Game Owner")]
        public async Task<GameMutationAdminResponse> ChangeOwner(Guid id, [FromBody] ChangeGameOwnerAdminRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new ChangeGameOwnerCommand(currentUser.IdentityId, id, request.NewOwnerId), cancellationToken);
            return commandResult.Map(r => mapper.MapToGameMutationResponse(r));
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/store-picture", Name = "Upload Store Picture Admin")]
        [EndpointSummary("Upload Store Picture")]
        public async Task<Result> UpdateStorePicture(Guid id, [FromForm] UpdateGameStorePictureAdminRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var storePicture = FileData.FromFormFile(request.StorePicture);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, storePicture), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/store-picture", Name = "Remove Store Picture Admin")]
        [EndpointSummary("Remove Store Picture")]
        public async Task<Result> RemoveStorePicture(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/artwork/{artworkId}", Name = "Update Artwork Admin")]
        [EndpointSummary("Update Artwork")]
        public async Task<Result> UpdateArtwork(Guid id, Guid artworkId, [FromForm] UpdateGameArtworkAdminRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var artwork = FileData.FromFormFile(request.Artwork);
            var commandResult = await mediator.Send(new UpdateGameArtworkCommand(currentUser.IdentityId, id, artworkId, artwork), cancellationToken);
            return commandResult.ToResult();
        }
    }
}
