using System.ComponentModel;

namespace WebApi.Features.Genres
{
    public record GetGenresParameters
    {
        [DefaultValue("")]
        public string Name { get; init; } = string.Empty;

        [DefaultValue(1)]
        public int PageNumber { get; init; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; init; } = 10;
    }
}
