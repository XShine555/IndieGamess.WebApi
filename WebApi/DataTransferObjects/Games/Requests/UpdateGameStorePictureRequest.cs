using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameStorePictureRequest
    {
        [Required]
        public required IFormFile StorePicture { get; set; }
    }
}
