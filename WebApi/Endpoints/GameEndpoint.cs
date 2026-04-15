using Application.Games.Commands;
using Application.Games.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Games;
using WebApi.Extensions;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games")]
    public class GameEndpoint(IMediator mediator, ILogger<GameEndpoint> logger, GameMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet]
        public async Task<PaginatedResponse<GameResponse>> Get( [FromQuery] GetGames query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageSize, query.PageSize), cancellationToken);
            return await mapper.MapToGamePaginatedResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{id}")]
        public async Task<GameResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost]
        [Authorize]
        public async Task<GameResponse> CreateGame( [FromForm] CreateGameRequest createGameRequest, CancellationToken cancellationToken,
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
            return await commandResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<Result> RemoveGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<GameResponse> UpdateGame(Guid id, UpdateGameRequest updateGameRequest, CancellationToken cancellationToken,
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
            return await commandResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/publish")]
        [Authorize]
        public async Task<Result> PublishGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PublishGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/genres")]
        [Authorize]
        public async Task<GameResponse> UpdateGenres(Guid id, UpdateGameGenresRequest updateGameGenresRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameGenresCommand(currentUser.IdentityId, id, updateGameGenresRequest.Genres), cancellationToken);
            return await commandResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/storePicture")]
        [Authorize]
        public async Task<Result> UpdateStorePicture(Guid id, [FromForm] IFormFile formFile, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var storePicture = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, storePicture), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/storePicture")]
        [Authorize]
        public async Task<Result> RemoveStorePicture(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToResult();
        }
    }
}