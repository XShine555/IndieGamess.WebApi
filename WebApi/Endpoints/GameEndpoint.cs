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
    [Tags("Games")]
    public class GameEndpoint(IMediator mediator, ILogger<GameEndpoint> logger, GameMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Games")]
        [EndpointSummary("Get Games")]
        public async Task<PaginatedResponse<GameListItemResponse>> Get([FromQuery] GetGamesRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageNumber, query.PageSize), cancellationToken);
            return await mapper.MapToGamePaginatedResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{id}", Name = "Get Game By Id")]
        [EndpointSummary("Get Game By Id")]
        public async Task<Result<GameResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Game")]
        [Consumes("multipart/form-data")]
        [EndpointSummary("Create Game")]
        [Authorize]
        public async Task<Result<GameMutationResponse>> CreateGame([FromForm] CreateGameRequest createGameRequest, CancellationToken cancellationToken,
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
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}", Name = "Remove By Id")]
        [EndpointSummary("Remove Game")]
        [Authorize]
        public async Task<Result> RemoveGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update By Id")]
        [EndpointSummary("Update Game")]
        [Authorize]
        public async Task<Result<GameMutationResponse>> UpdateGame(Guid id, UpdateGameRequest updateGameRequest, CancellationToken cancellationToken,
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
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/publish", Name = "Publish Game")]
        [EndpointSummary("Publish Game")]
        [Authorize]
        public async Task<Result> PublishGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PublishGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/genres", Name = "Update Genres")]
        [EndpointSummary("Update Genres")]
        [Authorize]
        public async Task<Result<GameGenresMutationResponse>> UpdateGenres(Guid id, UpdateGameGenresRequest updateGameGenresRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameGenresCommand(currentUser.IdentityId, id, updateGameGenresRequest.Genres), cancellationToken);
            return commandResult.Map(mapper.MapToGameGenresMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/store-picture", Name = "Upload Store Picture")]
        [EndpointSummary("Upload Store Picture")]
        [Authorize]
        public async Task<Result> UpdateStorePicture(Guid id, [FromForm] IFormFile formFile, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var storePicture = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, storePicture), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/store-picture", Name = "Remove Store Picture")]
        [EndpointSummary("Remove Store Picture")]
        [Authorize]
        public async Task<Result> RemoveStorePicture(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/artwork/{artworkId}", Name = "Update Artwork")]
        [EndpointSummary("Update Artwork")]
        [Authorize]
        public async Task<Result> UpdateArtwork(Guid id, Guid artworkId, [FromForm] IFormFile formFile, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var artwork = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateGameArtworkCommand(currentUser.IdentityId, id, artworkId, artwork), cancellationToken);
            return commandResult.ToResult();
        }
    }
}