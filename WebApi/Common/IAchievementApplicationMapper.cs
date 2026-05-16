using Application.Achievements.Responses;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Common
{
    public interface IAchievementApplicationMapper
    {
        Task<AchievementResponse> MapToAchievementResponse(ApplicationAchievement achievement, CancellationToken cancellationToken);
        Task<AchievementResponse> MapToUserAchievementResponseAsync(ApplicationUserAchievement achievement, CancellationToken cancellationToken);
        AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement);
        Task<AchievementDeveloperResponse> MapToAchievementDeveloperResponse(ApplicationAchievement applicationAchievement, CancellationToken cancellationToken);
    }
}
