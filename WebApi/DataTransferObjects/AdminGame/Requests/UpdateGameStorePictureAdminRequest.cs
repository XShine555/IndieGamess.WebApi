using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame.Requests
{
    public class UpdateGameStorePictureAdminRequest
    {
        [Required]
        public required IFormFile StorePicture { get; set; }
    }
}
