using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users
{
    public class CreateGameCollectionRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 1)]
        public required string Name { get; set; }
    }
}
