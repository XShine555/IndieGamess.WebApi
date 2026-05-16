using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame.Requests
{
    public class UpdateGameArtworkAdminRequest
    {
        [Required]
        public required IFormFile Artwork { get; set; }
    }
}
