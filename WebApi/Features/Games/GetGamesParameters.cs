using System.ComponentModel;

namespace WebApi.Features.Games;

public record GetGamesParameters
{
    [DefaultValue("")]
    public string Title { get; init; } = string.Empty;

    [DefaultValue(new int[0])]
    public int[] Genres { get; init; } = Array.Empty<int>();

    [DefaultValue(1)]
    public int PageNumber { get; init; } = 1;

    [DefaultValue(10)]
    public int PageSize { get; init; } = 10;
}
