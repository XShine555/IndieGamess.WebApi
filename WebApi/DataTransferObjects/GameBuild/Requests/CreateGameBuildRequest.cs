using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.GameBuild.Requests
{
    public class CreateGameBuildRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string VersionName { get; set; }
    }
}
