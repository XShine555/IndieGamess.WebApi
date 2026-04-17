using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame
{
    public class UpdateGameGenresAdminRequest
    {
        [Required]
        public required Guid[] Genres { get; set; }
    }
}
