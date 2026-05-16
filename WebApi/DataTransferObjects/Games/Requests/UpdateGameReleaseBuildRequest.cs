using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameReleaseBuildRequest
    {
        [Required]
        public Guid BuildId { get; set; }
    }
}
