using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.GameBuild.Requests
{
    public class UpdateExecutableFileRequest
    {
        [Required]
        public required string FilePath { get; set; }
    }
}
