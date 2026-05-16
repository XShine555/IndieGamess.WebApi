using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameArtworkRequest
    {
        [Required]
        public required IFormFile Artwork { get; set; }
    }
}
