using Application.Abstractions.Storage;
using Application.Games.Responses;
using Domain.Entities;
using WebApi.Features.Genres;
using WebApi.Features.Users;

namespace WebApi.Features.Games
{
    public sealed class GameResponseMapper(IS3Service s3Service)
    {
        public async Task<GameResponse> FromApplicationResponseAsync(ApplicationGame game, CancellationToken cancellationToken)
        {
            var pictures = await Task.WhenAll(
                game.Pictures.Where(p => p.ProcessingStatus == GamePictureProcessingStatus.Completed)
                .Select(picture =>
                    GameStorePictureResponse.FromApplicationResponseAsync(picture, s3Service, cancellationToken)));

            return new GameResponse(
                game.Id,
                game.Title,
                game.Description,
                UserSummary.FromApplicationSummaryResponse(game.ApplicationUserSummary),
                game.Genres.Select(GenreSummary.FromApplicationResponse).ToArray(),
                pictures);
        }
    }
}