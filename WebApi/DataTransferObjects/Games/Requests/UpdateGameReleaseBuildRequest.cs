using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameReleaseBuildRequest
    {
        [Required]
        public Guid GameId { get; set; }

        [Required]
        public Guid BuildId { get; set; }
    }
}
