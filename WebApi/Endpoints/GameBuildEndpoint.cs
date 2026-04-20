using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObjects.GameBuild.Requests;
using WebApi.DataTransferObjects.GameBuild.Responses;
using WebApi.Extensions;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games/{gameId}/game-builds")]
    [Tags("Game Builds")]
    public class GameBuildEndpoint(IMediator mediator, GameBuildMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Game Builds")]
        [EndpointSummary("Get Game Builds")]
        [Authorize]
        public async Task<Result<IReadOnlyCollection<GameBuildResponse>>> GetGameBuilds(Guid gameId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new global::Application.Games.Builds.Queries.GetGameBuildsQuery(currentUser.IdentityId, gameId), cancellationToken);
            return queryResult.Map(builds => (IReadOnlyCollection<GameBuildResponse>)builds.Select(mapper.MapToGameBuildResponse).ToArray());
        }

        [TranslateResultToActionResult]
        [HttpGet("{buildId}", Name = "Get Game Build By Id")]
        [EndpointSummary("Get Game Build By Id")]
        [Authorize]
        public async Task<Result<GameBuildResponse>> GetGameBuild(Guid gameId, Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new global::Application.Games.Builds.Queries.GetGameBuildsQuery(currentUser.IdentityId, gameId), cancellationToken);

            if (!queryResult.IsSuccess)
            {
                return Result<GameBuildResponse>.Error(new ErrorList(queryResult.Errors, queryResult.CorrelationId));
            }

            var build = queryResult.Value.FirstOrDefault(item => item.BuildId == buildId);
            return build is null
                ? Result<GameBuildResponse>.NotFound()
                : Result<GameBuildResponse>.Success(mapper.MapToGameBuildResponse(build));
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Game Build")]
        [EndpointSummary("Create Game Build")]
        [Authorize]
        public async Task<Result<GameBuildMutationResponse>> CreateGameBuild(Guid gameId, [FromBody] CreateGameBuildRequest request,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new global::Application.Games.Builds.Commands.CreateGameBuildCommand(currentUser.IdentityId, gameId, request.VersionName), cancellationToken);
            return commandResult.Map(mapper.MapToGameBuildMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPut("{buildId}", Name = "Update Game Build")]
        [EndpointSummary("Update Game Build")]
        [Authorize]
        public async Task<Result<GameBuildMutationResponse>> UpdateGameBuild(Guid gameId, Guid buildId, [FromBody] UpdateGameBuildRequest request,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new global::Application.Games.Builds.Commands.UpdateGameBuildCommand(currentUser.IdentityId, buildId, request.VersionName), cancellationToken);
            return commandResult.Map(mapper.MapToGameBuildMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{buildId}", Name = "Remove Game Build")]
        [EndpointSummary("Remove Game Build")]
        [Authorize]
        public async Task<Result> RemoveGameBuild(Guid gameId, Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new global::Application.Games.Builds.Commands.RemoveGameBuildCommand(currentUser.IdentityId, buildId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("{buildId}/upload", Name = "Get Url To Upload Files")]
        [EndpointSummary("Get Url To Upload Files")]
        [Authorize]
        public async Task<Result<IReadOnlyCollection<GameBuildUploadFileResponse>>> GetPresignUrl(Guid gameId, Guid buildId,
            [FromBody] GameBuildUploadFilesRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new global::Application.Games.Builds.Commands.PreSignGameFilesRequestCommand(currentUser.IdentityId, buildId, request.FilePaths), cancellationToken);
            return commandResult.Map(items => (IReadOnlyCollection<GameBuildUploadFileResponse>)items.Select(mapper.MapToGameBuildUploadFileResponse).ToArray());
        }

        [TranslateResultToActionResult]
        [HttpPost("{buildId}/complete", Name = "Complete Upload Game Build")]
        [EndpointSummary("Complete Upload Game Build")]
        [Authorize]
        public async Task<Result> CompleteUploadGameBuild(Guid gameId, Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new global::Application.Games.Builds.Commands.CompleteGameBuildCommand(currentUser.IdentityId, buildId), cancellationToken);
            return commandResult;
        }
    }
}
