using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGenre.Requests
{
    public class CreateGenreAdminRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
