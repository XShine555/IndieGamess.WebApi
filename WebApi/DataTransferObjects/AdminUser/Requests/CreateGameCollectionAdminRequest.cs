using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminUser.Requests
{
    public class CreateGameCollectionAdminRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 1)]
        public required string Name { get; set; }
    }
}
