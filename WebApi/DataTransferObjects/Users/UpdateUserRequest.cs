using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users
{
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string DisplayName { get; set; }
    }
}
