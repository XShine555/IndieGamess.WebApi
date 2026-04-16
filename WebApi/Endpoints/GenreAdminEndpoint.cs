using Application.Genres.Commands;
using Application.Genres.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGenre;
using WebApi.Mappers;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("admin/genre")]
    [Tags("Admin Genre")]
    [Authorize]
    public class GenreAdminEndpoint(IMediator mediator, AdminGenreMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Admin Genres")]
        [EndpointSummary("Get Admin Genres")]
        public async Task<PaginatedResponse<GenreListItemResponse>> Get([FromQuery] GetGenres query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenresQuery(query.Name, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGenrePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get Admin Genre By Id")]
        [EndpointSummary("Get Admin Genre By Id")]
        public async Task<GenreResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
            return mapper.MapToGenreResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Admin Genre")]
        [EndpointSummary("Create Admin Genre")]
        public async Task<GenreSummary> Create([FromBody] CreateGenreRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new CreateGenreCommand(request.Name), cancellationToken);
            return mapper.MapToGenreSummary(commandResult);
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update Admin Genre")]
        [EndpointSummary("Update Admin Genre")]
        public async Task<GenreSummary> Update(Guid id, [FromBody] UpdateGenreRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new UpdateGenreCommand(id, request.Name), cancellationToken);
            return mapper.MapToGenreSummary(commandResult);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}", Name = "Remove Admin Genre")]
        [EndpointSummary("Remove Admin Genre")]
        public async Task<Result> Remove(Guid id, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new RemoveGenreCommand(id), cancellationToken);
            return commandResult;
        }
    }
}
