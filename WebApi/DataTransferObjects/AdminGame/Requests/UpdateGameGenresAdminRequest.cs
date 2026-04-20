using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame.Requests
{
    public class UpdateGameGenresAdminRequest
    {
        [Required]
        [MinLength(1)]
        public required Guid[] Genres { get; set; }
    }
}
