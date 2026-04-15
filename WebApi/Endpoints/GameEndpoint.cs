using Application.Games.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObjects.Games;
using WebApi.Mappers;
using WebApi.Common;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("games")]
    public class GameEndpoint(IMediator mediator, ILogger<GameEndpoint> logger, GameMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet]
        public async Task<PaginatedResponse<GameResponse>> Get( [FromQuery] GetQuery query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(query.Title, query.Genres, query.PageSize, query.PageSize), cancellationToken);
            return await mapper.MapToGamePaginatedResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{id}")]
        public async Task<GameResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGameResponse(r, cancellationToken));
        }
    }
}