using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.GameBuild.Requests
{
    public class GameBuildUploadFilesRequest()
    {
        [Required]
        [MinLength(1)]
        public required string[] FilePaths { get; set; }
    }
}
