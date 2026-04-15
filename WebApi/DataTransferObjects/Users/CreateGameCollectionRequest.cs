using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users
{
    public class CreateGameCollectionRequest
    {
        [Required]
        [Range(1, 24)]
        public required string Name { get; set; }
    }
}
