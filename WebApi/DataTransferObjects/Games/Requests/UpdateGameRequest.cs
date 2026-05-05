using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameRequest
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
        [Range(0, 4000)]
        public decimal Discount { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}