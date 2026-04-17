using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame
{
    public class UpdateGameAdminRequest
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
        [Range(0, 100)]
        public decimal Discount { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}
