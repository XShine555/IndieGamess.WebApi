using Application.Abstractions.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Features.Games.Requests
{
    public record CreateGameRequest(
        [Required] [MinLength(3)]
        string Title,
        [Required] [MinLength(10)]
        string Description,
        [Required]
        IFormFile HeaderPicture,
        [Required]
        IFormFile CapsulePicture,
        [Required]
        IFormFile MainPicture)
    {
        [DefaultValue(new int[0] )]
        public int[] Genres { get; init; } = Array.Empty<int>();
    }
}
