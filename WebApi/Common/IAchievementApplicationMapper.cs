using Application.Abstractions.Common;
using Application.Achievements.Responses;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Common
{
    public interface IAchievementApplicationMapper
    {
        AchievementResponse MapToAchievementResponse(ApplicationAchievement achievement);
        AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement);
        PaginatedResponse<AchievementResponse> MapToAchievementPaginatedResponse(PaginatedApplicationResponse<ApplicationAchievement> paginated);
    }
}
