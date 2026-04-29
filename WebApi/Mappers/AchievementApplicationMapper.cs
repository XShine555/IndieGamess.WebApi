using Application.Abstractions.Common;
using Application.Achievements.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Mappers
{
    public class AchievementApplicationMapper : IAchievementApplicationMapper
    {
        public AchievementResponse MapToAchievementResponse(ApplicationAchievement achievement)
            => new(achievement.Id, achievement.GameId, achievement.Name, achievement.Description, achievement.CreatedAt, achievement.UpdatedAt);

        public AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement)
            => new(achievement.Id, achievement.GameId, achievement.Name, achievement.Description, achievement.CreatedAt);

        public PaginatedResponse<AchievementResponse> MapToAchievementPaginatedResponse(PaginatedApplicationResponse<ApplicationAchievement> paginated)
            => PaginatedResponse<AchievementResponse>.FromApplicationResponse(paginated, MapToAchievementResponse);
    }
}
