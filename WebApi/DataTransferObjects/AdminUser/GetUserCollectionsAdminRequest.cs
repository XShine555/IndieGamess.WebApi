using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminUser
{
    public class GetUserCollectionsAdminRequest
    {
        [Range(1, 2147483647)]
        public int PageNumber { get; set; } = 1;

        [Range(0, 50)]
        public int PageSize { get; set; } = 10;
    }
}
