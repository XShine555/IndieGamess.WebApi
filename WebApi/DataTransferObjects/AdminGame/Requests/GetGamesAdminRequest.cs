using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.AdminGame.Requests
{
    public class GetGamesAdminRequest
    {
        public string? Title { get; set; }

        public Guid[]? Genres { get; set; }

        [Range(1, 100)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}
