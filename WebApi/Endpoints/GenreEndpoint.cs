using Application.Genres.Queries;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Genres;
using WebApi.Mappers;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("genres")]
    public class GenreEndpoint(IMediator mediator, ILogger<GenreEndpoint> logger, GenreMapper mapper)
        : Controller
    {
        [HttpGet]
        public async Task<PaginatedResponse<GenreResponse>> Get( [FromQuery] GetGenres query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenresQuery(query.Name, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGenrePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
            return mapper.MapToGenreResponse(queryResult);
        }
    }
}