using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games
{
    public class CreateGameRequest
    {
        [Required]
        [Range(3, 24)]
        public required string Title { get; set; }

        [Required]
        [Range(1, 4096)]
        public required string Description { get; set; }

        [Required]
        [Range(0, 4000)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(1)]
        public required Guid[] Genres { get; set; }

        [Required]
        public required IFormFile CapsulePicture { get; set; }

        [Required]
        public required IFormFile HeaderPicture { get; set; }

        [Required]
        public required IFormFile MainPicture { get; set; }
    }
}
