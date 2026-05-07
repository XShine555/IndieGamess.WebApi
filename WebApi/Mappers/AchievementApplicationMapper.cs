using Application.Achievements.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Mappers
{
    public class AchievementApplicationMapper : IAchievementApplicationMapper
    {
        public AchievementResponse MapToAchievementResponse(ApplicationAchievement achievement)
        {
            return new AchievementResponse(
                achievement.Id,
                achievement.GameId,
                achievement.Name,
                achievement.Description,
                null,
                null,
                achievement.CreatedAt,
                achievement.UpdatedAt);
        }

        public AchievementResponse MapToUserAchievementResponse(ApplicationUserAchievement achievement)
        {
            return new AchievementResponse(
                achievement.AchievementId,
                achievement.GameId,
                achievement.Name,
                achievement.Description,
                achievement.IsUnlocked,
                achievement.UnlockedAt,
                null,
                null);
        }

        public AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement)
        {
            return new AchievementMutationResponse(
                achievement.Id,
                achievement.GameId,
                achievement.Name,
                achievement.Description);
        }
    }
}
