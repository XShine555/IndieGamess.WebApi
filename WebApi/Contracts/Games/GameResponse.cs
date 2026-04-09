using Application.Games.Responses;
using WebApi.Contracts.Genres;
using WebApi.Contracts.Users;

namespace WebApi.Contracts.Games
{
    public record GameResponse(
        int Id,
        string Title,
        string Description,
        UserSummary Owner,
        ICollection<GenreSummary> Genres)
    {
        public static GameResponse FromApplicationResponse(ApplicationGame game)
        {
            return new GameResponse(
                game.Id,
                game.Title,
                game.Description,
                UserSummary.FromApplicationSummaryResponse(game.ApplicationUserSummary),
                game.Genres.Select(GenreSummary.FromApplicationResponse).ToList());
        }
    }
}