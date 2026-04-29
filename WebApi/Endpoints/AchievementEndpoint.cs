using Application.Achievements.Commands;
using Application.Achievements.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Common;
using WebApi.DataTransferObjects.Achievements.Requests;
using WebApi.DataTransferObjects.Achievements.Responses;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Tags("Achievements")]
    public class AchievementEndpoint(IMediator mediator, IAchievementApplicationMapper mapper) : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet("games/{gameId}/achievements", Name = "Get Game Achievements")]
        [EndpointSummary("Get Game Achievements")]
        public async Task<PaginatedResponse<AchievementResponse>> GetByGame(
            Guid gameId,
            [FromQuery] GetAchievementsRequest query,
            CancellationToken cancellationToken)
        {
            var userIdClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = userIdClaim is not null ? Guid.Parse(userIdClaim) : (Guid?)null;
            var result = await mediator.Send(new GetAchievementsByGameQuery(gameId, userId, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToAchievementPaginatedResponse(result);
        }

        [TranslateResultToActionResult]
        [HttpGet("achievements/{achievementId}", Name = "Get Achievement By Id")]
        [EndpointSummary("Get Achievement By Id")]
        public async Task<Result<AchievementResponse>> GetById(Guid achievementId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAchievementByIdQuery(achievementId), cancellationToken);
            return result.Map(mapper.MapToAchievementResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("games/{gameId}/achievements", Name = "Create Achievement")]
        [EndpointSummary("Create Achievement")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<Result<AchievementMutationResponse>> Create(
            Guid gameId,
            [FromForm] CreateAchievementRequest request,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var picture = FileData.FromFormFile(request.Picture);
            var result = await mediator.Send(new CreateAchievementCommand(gameId, currentUser.IdentityId, request.Name, request.Description, picture), cancellationToken);
            return result.Map(mapper.MapToAchievementMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("achievements/{achievementId}/unlock", Name = "Unlock Achievement")]
        [EndpointSummary("Unlock Achievement")]
        [Authorize]
        public async Task<Result> Unlock(
            Guid achievementId,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            return await mediator.Send(new UnlockAchievementCommand(currentUser.IdentityId, achievementId), cancellationToken);
        }
    }
}
