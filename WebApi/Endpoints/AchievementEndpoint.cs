using System.Security.Claims;
using Application.Achievements.Commands;
using Application.Achievements.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Achievements.Requests;
using WebApi.DataTransferObjects.Achievements.Responses;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games")]
    [Tags("Achievements")]
    public class AchievementEndpoint(IMediator mediator, IAchievementApplicationMapper mapper)
        : ControllerBase
    {
        [HttpGet("{gameId}/achievements", Name = "Get Game Achievements")]
        [EndpointSummary("Get Game Achievements")]
        public async Task<PaginatedResponse<AchievementResponse>> GetAchievements(
            Guid gameId,
            [FromQuery] GetAchievementsRequest request,
            CancellationToken cancellationToken)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userIdString, out var userId))
                {
                    var userAchievements = await mediator.Send(
                        new GetUserAchievementsByGameQuery(userId, gameId), cancellationToken);
                    return new PaginatedResponse<AchievementResponse>(
                        userAchievements.Select(mapper.MapToUserAchievementResponse).ToList(),
                        1,
                        userAchievements.Count,
                        1,
                        userAchievements.Count,
                        false,
                        false);
                }
            }

            var result = await mediator.Send(
                new GetAchievementsByGameQuery(gameId, request.PageNumber, request.PageSize), cancellationToken);
            return PaginatedResponse<AchievementResponse>.FromApplicationResponse(result, mapper.MapToAchievementResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{gameId}/achievements", Name = "Create Achievement")]
        [EndpointSummary("Create Achievement")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<Result<AchievementMutationResponse>> CreateAchievement(
            Guid gameId,
            [FromForm] CreateAchievementRequest request,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var picture = FileData.FromFormFile(request.Picture);
            var commandResult = await mediator.Send(
                new CreateAchievementCommand(gameId, currentUser.IdentityId, request.Name, request.Description, picture),
                cancellationToken);
            return commandResult.Map(mapper.MapToAchievementMutationResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("~/achievements/{achievementId}/unlock", Name = "Unlock Achievement")]
        [EndpointSummary("Unlock Achievement")]
        [Authorize]
        public async Task<Result> UnlockAchievement(
            Guid achievementId,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            return await mediator.Send(
                new UnlockAchievementCommand(currentUser.IdentityId, achievementId), cancellationToken);
        }
    }
}
