using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games
{
    public class GetGames
    {
        public string Title { get; set; } = string.Empty;

        public Guid[] Genres { get; set; } = Array.Empty<Guid>();

        [Range(1, 2147483647)]
        public int PageNumber { get; set; } = 1;

        [Range(0, 50)]
        public int PageSize { get; set; } = 10;
    }
}
