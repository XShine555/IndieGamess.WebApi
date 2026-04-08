using Application.Genres.Commands;
using Application.Genres.Queries;
using Application.Genres.Responses;
using Ardalis.Result;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Contracts.Genres;

namespace WebApi.Endpoints
{
    public class GenreEndpoint
    {
        public static void Map(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/genres")
                .WithTags("Genres");

            group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
                [FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
            {
                var queryResult = await mediator.Send(new GetGenresByNameQuery(name, pageNumber, pageSize), cancellationToken);
                return Results.Ok(
                    PaginatedResponse<ApplicationGenre>.FromApplicationResponse(
                        queryResult,
                        GenreResponse.FromApplicationResponse));
            } )
                .WithSummary("Get Genres By Name");

            group.MapGet("/{id:guid}", async (IMediator mediator, CancellationToken cancellationToken, Guid id) =>
            {
                var queryResult = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
                return Results.Ok(GenreResponse.FromApplicationResponse(queryResult));
            } )
                .WithSummary("Get Genre By Id");

            group.MapPost("/", async (IMediator mediator, CancellationToken cancellationToken, [FromBody] CreateGenreRequest request) =>
            {
                var queryResult = await mediator.Send(new CreateGenreCommand(request.Name), cancellationToken);
                return queryResult.Map(r => Results.Created($"/genres/{r.Id}", GenreResponse.FromApplicationResponse(r)));
            } )
                .WithSummary("Create Genre")
                .RequireAuthorization();

            group.MapDelete("/{id:guid}", async (IMediator mediator, CancellationToken cancellationToken, Guid id) =>
            {
                var queryResult = await mediator.Send(new RemoveGenreCommand(id), cancellationToken);
                return queryResult.Map(r => Results.NoContent());
            } )
                .WithSummary("Delete Genre")
                .RequireAuthorization();

            group.MapPut("/{id:guid}", async (IMediator mediator, CancellationToken cancellationToken, Guid id, [FromBody] UpdateGenreRequest request) =>
            {
                var queryResult = await mediator.Send(new UpdateGenreCommand(id, request.Name), cancellationToken);
                return queryResult.Map(r => Results.Ok(GenreResponse.FromApplicationResponse(r)));
            } )
                .WithSummary("Update Genre")
                .RequireAuthorization();
        }
    }
}