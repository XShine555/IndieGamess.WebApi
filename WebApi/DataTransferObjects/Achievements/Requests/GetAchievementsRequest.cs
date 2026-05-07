using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Achievements.Requests
{
    public class GetAchievementsRequest
    {
        [Range(1, 100)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
