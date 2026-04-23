using Application.Games.Builds.Commands;
using Application.Games.Builds.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObjects.GameBuild.Requests;
using WebApi.DataTransferObjects.GameBuild.Responses;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("game-builds")]
    [Tags("Game Builds")]
    public class GameBuildEndpoint(IMediator mediator, IGameBuildMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet("{buildId}", Name = "Get Game Build By Id As User")]
        [EndpointSummary("Get Game Build By Id As User")]
        [Authorize]
        public async Task<Result<GameBuildResponse>> GetGameBuild(Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetGameBuildByIdQuery(currentUser.IdentityId, buildId), cancellationToken);
            return queryResult.Map(mapper.MapToGameBuildResponse);
        }

        [TranslateResultToActionResult]
        [HttpPut("{buildId}", Name = "Update Game Build")]
        [EndpointSummary("Update Game Build")]
        [Authorize]
        public async Task<Result<GameBuildMutationResponse>> UpdateGameBuild(Guid buildId, [FromBody] UpdateGameBuildRequest request,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateGameBuildCommand(currentUser.IdentityId, buildId, request.VersionName), cancellationToken);
            return commandResult.Map(mapper.MapToGameBuildMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{buildId}", Name = "Remove Game Build")]
        [EndpointSummary("Remove Game Build")]
        [Authorize]
        public async Task<Result> RemoveGameBuild(Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameBuildCommand(currentUser.IdentityId, buildId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("{buildId}/upload", Name = "Get Url To Upload Files")]
        [EndpointSummary("Get Url To Upload Files")]
        [Authorize]
        public async Task<Result<IReadOnlyCollection<GameBuildUploadFileResponse>>> GetPresignUrl(Guid buildId,
            [FromBody] GameBuildUploadFilesRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PreSignGameFilesRequestCommand(currentUser.IdentityId, buildId, request.FilePaths), cancellationToken);
            return commandResult.Map(items =>
                (IReadOnlyCollection<GameBuildUploadFileResponse>)items.Select(mapper.MapToGameBuildUploadFileResponse).ToArray());
        }

        [TranslateResultToActionResult]
        [HttpPost("{buildId}/complete", Name = "Complete Upload Game Build")]
        [EndpointSummary("Complete Upload Game Build")]
        [Authorize]
        public async Task<Result> CompleteUploadGameBuild(Guid buildId, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CompleteGameBuildCommand(currentUser.IdentityId, buildId), cancellationToken);
            return commandResult;
        }
    }
}
