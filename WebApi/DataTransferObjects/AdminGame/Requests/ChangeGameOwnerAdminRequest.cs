using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame.Requests
{
    public class ChangeGameOwnerAdminRequest
    {
        [Required]
        public Guid NewOwnerId { get; set; }
    }
}
