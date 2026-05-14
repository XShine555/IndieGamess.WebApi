using Application.Abstractions.Storage;
using Application.Achievements.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.Achievements.Responses;

namespace WebApi.Mappers
{
    public class AchievementApplicationMapper(IS3Service s3Service) : SignedUrlMapper(s3Service), IAchievementApplicationMapper
    {
        public AchievementResponse MapToAchievementResponse(ApplicationAchievement achievement)
        {
            return new AchievementResponse(
                achievement.Id,
                achievement.GameId,
                achievement.Name,
                achievement.Description,
                null,
                null);
        }

        public AchievementResponse MapToUserAchievementResponse(ApplicationUserAchievement achievement)
        {
            return new AchievementResponse(
                achievement.AchievementId,
                achievement.GameId,
                achievement.Name,
                achievement.Description,
                achievement.IsUnlocked,
                achievement.UnlockedAt);
        }

        public AchievementMutationResponse MapToAchievementMutationResponse(ApplicationAchievement achievement)
        {
            return new AchievementMutationResponse(
                achievement.Id,
                achievement.GameId,
                achievement.Name,
                achievement.Description);
        }

        public async Task<AchievementDeveloperResponse> MapToAchievementDeveloperResponse(ApplicationAchievement applicationAchievement, CancellationToken cancellationToken)
        {
            string? smallPictureUrl = null;
            if (!string.IsNullOrEmpty(applicationAchievement.SmallPicturePath))
                smallPictureUrl = await CreateSignedUrlAsync(applicationAchievement.SmallPicturePath, cancellationToken);

            string? mediumPictureUrl = null;
            if (!string.IsNullOrEmpty(applicationAchievement.MediumPicturePath))
                mediumPictureUrl = await CreateSignedUrlAsync(applicationAchievement.MediumPicturePath, cancellationToken);

            string? largePictureUrl = null;
            if (!string.IsNullOrEmpty(applicationAchievement.LargePicturePath))
                largePictureUrl = await CreateSignedUrlAsync(applicationAchievement.LargePicturePath, cancellationToken);

            return new AchievementDeveloperResponse(
                applicationAchievement.Id,
                applicationAchievement.GameId,
                applicationAchievement.Name,
                applicationAchievement.Description,
                applicationAchievement.Status.ToString(),
                applicationAchievement.IsPublished,
                smallPictureUrl,
                mediumPictureUrl,
                largePictureUrl);
        }
    }
}
