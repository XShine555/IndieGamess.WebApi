using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObjects.Games
{
    public class UpdateGameGenresRequest
    {
        [Required]
        public required Guid[] Genres { get; set; }
    }
}