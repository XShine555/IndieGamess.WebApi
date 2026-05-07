using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Achievements.Requests
{
    public class CreateAchievementRequest
    {
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 1)]
        public required string Description { get; set; }

        [Required]
        public required IFormFile Picture { get; set; }
    }
}
