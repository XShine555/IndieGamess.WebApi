using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame
{
    public class ChangeGameOwnerAdminRequest
    {
        [Required]
        public Guid NewOwnerId { get; set; }
    }
}
