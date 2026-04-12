using Application.Games.Responses;
using WebApi.Features.Genres;
using WebApi.Features.Users;

namespace WebApi.Features.Games;

public record GameResponse(
    int Id,
    string Title,
    string Description,
    UserSummary Owner,
    IReadOnlyCollection<GenreSummary> Genres)
{
    public static GameResponse FromApplicationResponse(ApplicationGame game)
    {
        return new GameResponse(
            game.Id,
            game.Title,
            game.Description,
            UserSummary.FromApplicationSummaryResponse(game.ApplicationUserSummary),
            game.Genres.Select(GenreSummary.FromApplicationResponse).ToArray());
    }
}
