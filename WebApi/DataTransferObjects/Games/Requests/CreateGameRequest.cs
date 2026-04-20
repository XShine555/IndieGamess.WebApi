using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class CreateGameRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string Title { get; set; }

        [Required]
        [StringLength(4096, MinimumLength = 1)]
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
