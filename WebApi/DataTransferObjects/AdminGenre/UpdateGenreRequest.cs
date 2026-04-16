using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGenre
{
    public class UpdateGenreRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
