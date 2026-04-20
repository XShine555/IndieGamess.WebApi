using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games.Requests
{
    public class UpdateGameGenresRequest
    {
        [Required]
        [MinLength(1)]
        public required Guid[] Genres { get; set; }
    }
}