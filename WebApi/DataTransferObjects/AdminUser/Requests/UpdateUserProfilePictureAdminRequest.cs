using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminUser.Requests
{
    public class UpdateUserProfilePictureAdminRequest
    {
        [Required]
        public required IFormFile ProfilePicture { get; set; }
    }
}
