using Application.Games.Catalog.Commands;
using Application.Games.Catalog.Queries;
using Application.Games.Builds.Commands;
using Application.Games.Builds.Queries;
using Application.Games.Media.Commands;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Requests;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.GameBuild.Requests;
using WebApi.DataTransferObjects.GameBuild.Responses;
using WebApi.Extensions;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games")]
    [Tags("Games")]
    public class GameEndpoint(IMediator mediator, IGameApplicationMapper mapper, IGameApplicationBuildMapper gameBuildMapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Store Games")]
        [EndpointSummary("Get Store Games")]
        public async Task<PaginatedResponse<GameListItemResponse>> Get( [FromQuery] GetGamesRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageNumber, query.PageSize), cancellationToken);
            return await mapper.MapToGameListPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{gameId}", Name = "Get Store Game By Id")]
        [EndpointSummary("Get Store Game By Id")]
        public async Task<Result<GameResponse>> GetById(Guid gameId, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(gameId), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Game")]
        [Consumes("multipart/form-data")]
        [EndpointSummary("Create Game")]
        [Authorize]
        public async Task<Result<GameMutationResponse>> CreateGame( [FromForm] CreateGameRequest createGameRequest, CancellationToken cancellationToken,
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
        [HttpDelete("{gameId}", Name = "Remove By Id")]
        [EndpointSummary("Remove Game")]
        [Authorize]
        public async Task<Result> RemoveGame(Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{gameId}", Name = "Update By Id")]
        [EndpointSummary("Update Game")]
        [Authorize]
        public async Task<Result<GameMutationResponse>> UpdateGame(Guid gameId, UpdateGameRequest updateGameRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameCommand(
                currentUser.IdentityId,
                gameId,
                updateGameRequest.Title,
                updateGameRequest.Description,
                updateGameRequest.Price,
                updateGameRequest.Discount,
                updateGameRequest.IsPublic), cancellationToken);
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPatch("{gameId}/release-build", Name = "Update Release Build")]
        [EndpointSummary("Update Release Build")]
        [Authorize]
        public async Task<Result<GameMutationResponse>> UpdateReleaseBuild(Guid gameId, [FromBody] UpdateGameReleaseBuildRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameReleaseBuildCommand(currentUser.IdentityId, gameId, request.BuildId), cancellationToken);
            return commandResult.Map(mapper.MapToGameMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{gameId}/publish", Name = "Publish Game")]
        [EndpointSummary("Publish Game")]
        [Authorize]
        public async Task<Result> PublishGame(Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PublishGameCommand(currentUser.IdentityId, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{gameId}/genres", Name = "Update Genres")]
        [EndpointSummary("Update Genres")]
        [Authorize]
        public async Task<Result<GameGenresMutationResponse>> UpdateGenres(Guid gameId, UpdateGameGenresRequest updateGameGenresRequest, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameGenresCommand(currentUser.IdentityId, gameId, updateGameGenresRequest.Genres), cancellationToken);
            return commandResult.Map(mapper.MapToGameGenresMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{gameId}/store-picture", Name = "Upload Store Picture")]
        [EndpointSummary("Upload Store Picture")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<Result> UpdateStorePicture(Guid gameId, [FromForm] UpdateGameStorePictureRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var storePicture = FileData.FromFormFile(request.StorePicture);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, gameId, storePicture), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpDelete("developer/store-picture/{pictureId}", Name = "Remove Store Picture")]
        [EndpointSummary("Remove Store Picture")]
        [Authorize]
        public async Task<Result> RemoveStorePicture(Guid pictureId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, pictureId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPatch("{gameId}/artwork/{artworkId}", Name = "Update Artwork")]
        [EndpointSummary("Update Artwork")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<Result> UpdateArtwork(Guid gameId, Guid artworkId, [FromForm] UpdateGameArtworkRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var artwork = FileData.FromFormFile(request.Artwork);
            var commandResult = await mediator.Send(new UpdateGameArtworkCommand(currentUser.IdentityId, gameId, artworkId, artwork), cancellationToken);
            return commandResult.ToResult();
        }

        [TranslateResultToActionResult]
        [HttpGet("{gameId}/builds", Name = "Get Game Builds As User")]
        [EndpointSummary("Get Game Builds As User")]
        [Authorize]
        public async Task<Result<PaginatedResponse<GameBuildAsUserListItemResponse> >> GetGameBuildsAsUser( [FromQuery] GetGameBuildsRequest request,
            Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetGameBuildsAsUserQuery(currentUser.IdentityId, gameId, request.Title, request.PageNumber, request.PageSize),
                cancellationToken);
            return queryResult.Map(builds => PaginatedResponse<GameBuildAsUserListItemResponse>.FromApplicationResponse(
                builds,
                gameBuildMapper.MapToGameBuildAsUserListItemResponse
            ));
        }

        [TranslateResultToActionResult]
        [HttpGet("developer/{gameId}/builds", Name = "Get Game Builds As Developer")]
        [EndpointSummary("Get Game Builds As Developer")]
        [Authorize]
        public async Task<Result<PaginatedResponse<GameBuildAsDeveloperListItemResponse>> > GetGameBuildsAsDeveloper(Guid gameId,
            [FromQuery] GetGameBuildsRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetGameBuildsAsDeveloperQuery(gameId, currentUser.IdentityId, request.Title, request.PageNumber, request.PageSize),
                cancellationToken);
            return queryResult.Map(builds => PaginatedResponse<GameBuildAsDeveloperListItemResponse>.FromApplicationResponse(
                builds,
                gameBuildMapper.MapToGameBuildAsDeveloperListItemResponse
            ));
        }

        [TranslateResultToActionResult]
        [HttpPost("{gameId}/builds", Name = "Create Game Build")]
        [EndpointSummary("Create Game Build")]
        [Authorize]
        public async Task<Result<GameBuildMutationResponse>> CreateGameBuild(Guid gameId, [FromBody] CreateGameBuildRequest request,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CreateGameBuildCommand(currentUser.IdentityId, gameId, request.VersionName), cancellationToken);
            return commandResult.Map(gameBuildMapper.MapToGameBuildMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpGet("developer/{gameId}", Name = "Get Game By Id As Developer")]
        [EndpointSummary("Get Game By Id As Developer")]
        [Authorize]
        public async Task<Result<DeveloperGameResponse>> GetGameAsDeveloper(Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(gameId, GameCatalogQueryMode.Developer), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToDeveloperGameResponse(r, cancellationToken));
        }
    }
}