using Application.Genres.Commands;
using Application.Genres.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminGenre.Requests;
using WebApi.DataTransferObjects.AdminGenre.Responses;
using WebApi.Mappers;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("admin/genre")]
    [Tags("Admin Genre")]
    [Authorize]
    public class GenreAdminEndpoint(IMediator mediator, IAdminGenreMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Genres Admin")]
        [EndpointSummary("Get Genres")]
        public async Task<PaginatedResponse<GenreListItemAdminResponse>> Get( [FromQuery] GetGenresAdminRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenresQuery(query.Name, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGenrePaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get Genre By Id Admin")]
        [EndpointSummary("Get Genre By Id")]
        public async Task<GenreAdminResponse> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
            return mapper.MapToGenreResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpPost(Name = "Create Genre Admin")]
        [EndpointSummary("Create Genre")]
        public async Task<GenreAdminSummary> Create( [FromBody] CreateGenreAdminRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new CreateGenreCommand(request.Name), cancellationToken);
            return mapper.MapToGenreSummary(commandResult);
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update Genre Admin")]
        [EndpointSummary("Update Genre")]
        public async Task<GenreAdminSummary> Update(Guid id, [FromBody] UpdateGenreAdminRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new UpdateGenreCommand(id, request.Name), cancellationToken);
            return mapper.MapToGenreSummary(commandResult);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}", Name = "Remove Genre Admin")]
        [EndpointSummary("Remove Genre")]
        public async Task<Result> Remove(Guid id, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new RemoveGenreCommand(id), cancellationToken);
            return commandResult;
        }
    }
}
