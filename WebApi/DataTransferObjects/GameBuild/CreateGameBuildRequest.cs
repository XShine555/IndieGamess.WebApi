using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.GameBuild
{
    public class CreateGameBuildRequest
    {
        [Required]
        public Guid GameId { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string VersionName { get; set; }
    }
}
