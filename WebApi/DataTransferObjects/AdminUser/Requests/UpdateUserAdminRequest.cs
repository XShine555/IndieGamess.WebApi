using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminUser.Requests
{
    public class UpdateUserAdminRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public required string DisplayName { get; set; }
    }
}
