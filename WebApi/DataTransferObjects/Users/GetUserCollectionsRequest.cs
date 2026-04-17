using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Users
{
    public class GetUserCollectionsRequest
    {
        [Range(1, 2147483647)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 2147483647)]
        public int PageSize { get; set; } = 10;
    }
}
