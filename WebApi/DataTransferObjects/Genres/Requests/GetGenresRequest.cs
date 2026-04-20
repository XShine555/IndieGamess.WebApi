using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Genres.Requests
{
    public class GetGenresRequest
    {
        public string Name { get; set; } = string.Empty;

        [Range(1, 2147483647)]
        public int PageNumber { get; set; } = 1;

        [Range(0, 50)]
        public int PageSize { get; set; } = 10;
    }
}
