using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users.Requests
{
    public class UpdateUserProfilePictureRequest
    {
        [Required]
        public required IFormFile ProfilePicture { get; set; }
    }
}