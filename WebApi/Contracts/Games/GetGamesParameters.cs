using System.ComponentModel;

namespace WebApi.Contracts.Games
{
    public record GetGamesParameters
    {
        [DefaultValue("")]
        public string Title { get; set; } = string.Empty;

        [DefaultValue(new int[0] )]
        public int[] Genres { get; set; } = Array.Empty<int>();

        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
