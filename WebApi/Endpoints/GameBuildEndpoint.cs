using Application.Games.Commands;
using Application.Games.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObjects.GameBuild;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games/{gameId}/game-builds/")]
    [Tags("Game Builds")]
    public class GameBuildEndpoint(IMediator mediator)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpPost]
        [EndpointSummary("Create Game Build")]
        [Authorize]
        public async Task<Result<GameBuildMutationResponse>> CreateGameBuild(Guid gameId, CreateGameBuildRequest request, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CreateGameBuildCommand(currentUser.IdentityId, gameId, request.VersionName));
            return commandResult.Map(r => new GameBuildMutationResponse(r.BuildId, gameId, r.VersionName));
        }

        [HttpGet("{buildId}")]
        [EndpointSummary("Get Game Build Details")]
        [Authorize]
        public async Task<Result<GetGameBuildRequest>> GetGameBuild(Guid buildId, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new GetGameBuildsQuery(currentUser.IdentityId, buildId));
            return commandResult;
        }

        [HttpPost("{buildId}/upload")]
        [EndpointSummary("Get Url To Upload Files")]
        [Authorize]
        public async Task<IActionResult> GetPresignUrl(Guid buildId, GameBuildUploadFilesRequest request,
            [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PreSignGameFilesRequestCommand(currentUser.IdentityId, buildId, request.FilePaths));
            var result = new List<GameBuildUploadFilesResponse>();
            foreach (var item in commandResult.Value)
            {
                result.Add(new GameBuildUploadFilesResponse(
                    item.OriginalFilePath, item.UploadUrl) );
            }
            return Ok(result);
        }

        [TranslateResultToActionResult]
        [HttpPost("{buildId}/Complete")]
        [EndpointSummary("Complete Upload Game Build")]
        [Authorize]
        public async Task<Result> CompleteUploadGameBuild(Guid buildId, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CompleteGameBuildCommand(currentUser.IdentityId, buildId));
            return commandResult;
        }
    }
}
