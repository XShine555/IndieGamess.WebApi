using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users
{
    public class GetUsersRequest
    {
        public string Username { get; set; } = string.Empty;

        [Range(1, 2147483647)]
        public int PageNumber { get; set; } = 1;

        [Range(0, 50)]
        public int PageSize { get; set; } = 10;
    }
}
