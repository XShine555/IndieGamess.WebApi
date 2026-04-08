using System.ComponentModel;

namespace WebApi.Contracts.Genres
{
    public record GetGenresParameters
    {
        [DefaultValue("")]
        public string Name { get; set; } = string.Empty;

        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
