using Application.Games.Commands;
using Application.Games.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGame;
using WebApi.Extensions;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("admin/game")]
    [Tags("Admin Game")]
    [Authorize]
    public class GameAdminEndpoint(IMediator mediator, ILogger<GameAdminEndpoint> logger, AdminGameMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Admin Games")]
        [EndpointSummary("Get Admin Games")]
        public async Task<PaginatedResponse<GameListItemAdminResponse>> Get( [FromQuery] GetGamesAdminRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGamePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get Admin Game By Id")]
        [EndpointSummary("Get Admin Game By Id")]
        public async Task<GameAdminResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Admin Game")]
        [EndpointSummary("Create Admin Game")]
        public async Task<GameMutationAdminResponse> CreateGame( [FromForm] CreateGameAdminRequest createGameRequest, CancellationToken cancellationToken,
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
        [HttpDelete("{id}", Name = "Remove Admin Game")]
        [EndpointSummary("Remove Admin Game")]
        public async Task<Result> RemoveGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update Admin Game")]
        [EndpointSummary("Update Admin Game")]
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
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/publish", Name = "Publish Admin Game")]
        [EndpointSummary("Publish Admin Game")]
        public async Task<Result> PublishGame(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PublishGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/genres", Name = "Update Admin Game Genres")]
        [EndpointSummary("Update Admin Game Genres")]
        public async Task<GameGenresMutationAdminResponse> UpdateGenres(Guid id, [FromBody] UpdateGameGenresAdminRequest updateGameGenresRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameGenresCommand(currentUser.IdentityId, id, updateGameGenresRequest.Genres), cancellationToken);
            return commandResult.Map(mapper.MapToGameGenresMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/owner", Name = "Change Admin Game Owner")]
        [EndpointSummary("Change Admin Game Owner")]
        public async Task<GameMutationAdminResponse> ChangeOwner(Guid id, [FromBody] ChangeGameOwnerAdminRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new ChangeGameOwnerCommand(currentUser.IdentityId, id, request.NewOwnerId), cancellationToken);
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/store-picture", Name = "Upload Admin Store Picture")]
        [EndpointSummary("Upload Admin Store Picture")]
        public async Task<Result> UpdateStorePicture(Guid id, [FromForm] IFormFile formFile, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var storePicture = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, storePicture), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/store-picture", Name = "Remove Admin Store Picture")]
        [EndpointSummary("Remove Admin Store Picture")]
        public async Task<Result> RemoveStorePicture(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/artwork/{artworkId}", Name = "Update Admin Artwork")]
        [EndpointSummary("Update Admin Artwork")]
        public async Task<Result> UpdateArtwork(Guid id, Guid artworkId, [FromForm] IFormFile formFile, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var artwork = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateGameArtworkCommand(currentUser.IdentityId, id, artworkId, artwork), cancellationToken);
            return commandResult.ToResult();
        }
    }
}
