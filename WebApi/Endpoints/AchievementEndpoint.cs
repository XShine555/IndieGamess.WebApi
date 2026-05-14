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
using WebApi.Extensions;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games")]
    [Tags("Achievements")]
    public class AchievementEndpoint(IMediator mediator, IAchievementApplicationMapper mapper)
        : ControllerBase
    {
        [HttpGet("{gameId}/achievements", Name = "Get Game Achievement as Developer")]
        [EndpointSummary("Get Game Achievements As Developer")]
        [Authorize]
        public async Task<Result<AchievementDeveloperResponse[]>> GetAchievements(
            Guid gameId,
            [FromServices] ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAchievementsByGameAsDeveloperQuery(
                gameId,
                currentUser.IdentityId), cancellationToken);
            if (!result.IsSuccess)
                return result.ToResult();

            var achievementResponses = new List<AchievementDeveloperResponse>();
            foreach (var achievement in result.Value)
            {
                achievementResponses.Add(await mapper.MapToAchievementDeveloperResponse(achievement, cancellationToken));
            }
            return result.Map(_ => achievementResponses.ToArray());
        }

        [HttpGet("{gameId}/achievements/{userId}")]
        [EndpointSummary("Get User Achievements for Game")]
        public async Task<IReadOnlyList<AchievementResponse>> GetUserAchievementsForGame(
            Guid gameId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserAchievementsByGameQuery(
                userId, gameId), cancellationToken);
            var achievementResponses = new List<AchievementResponse>();
            foreach (var achievement in result)
            {
                achievementResponses.Add(mapper.MapToUserAchievementResponse(achievement));
            }
            return achievementResponses;
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
        [HttpDelete("achievement/{achievementId}", Name = "Delete Achievement")]
        [EndpointSummary("Delete Achievement")]
        [Authorize]
        public async Task<Result> DeleteAchievement(
            Guid achievementId,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var result = await mediator.Send(new RemoveAchievementCommand(currentUser.IdentityId, achievementId), cancellationToken);
            return result;
        }

        [TranslateResultToActionResult]
        [HttpPost("{achievementId}/unlock", Name = "Unlock Achievement")]
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

        [TranslateResultToActionResult]
        [HttpPost("/achievement/{achievementId}/publish", Name = "Publish Achievement")]
        [EndpointSummary("Publish Achievement")]
        [Authorize]
        public async Task<Result> PublishAchievement(
            Guid achievementId,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            return await mediator.Send(
                new PublishAchievementCommand(currentUser.IdentityId, achievementId), cancellationToken);
        }
    }
}
