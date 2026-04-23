using Application.Genres.Queries;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Genres.Requests;
using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.Mappers;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("genres")]
    [Tags("Genres")]
    public class GenreEndpoint(IMediator mediator, IGenreMapper mapper)
        : Controller
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Genres")]
        [EndpointSummary("Get Genres")]
        public async Task<PaginatedResponse<GenreListItemResponse>> Get( [FromQuery] GetGenresRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenresQuery(query.Name, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGenrePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{genreId}", Name = "Get Genre By Id")]
        [EndpointSummary("Get Genre By Id")]
        public async Task<ActionResult<GenreResponse>> GetById(Guid genreId, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenreByIdQuery(genreId), cancellationToken);
            return mapper.MapToGenreResponse(queryResult);
        }
    }
}