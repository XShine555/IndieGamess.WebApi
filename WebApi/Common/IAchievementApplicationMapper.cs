using Application.Achievements.Responses;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Common
{
    public interface IAchievementApplicationMapper
    {
        AchievementResponse MapToAchievementResponse(ApplicationAchievement achievement);
        AchievementResponse MapToUserAchievementResponse(ApplicationUserAchievement achievement);
        AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement);
    }
}
