using Application.Genres.Commands;
using Application.Genres.Queries;
using Application.Genres.Responses;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Features.Genres.Parameters;
using WebApi.Features.Genres.Requests;
using WebApi.Features.Genres.Responses;

namespace WebApi.Features.Genres
{
    public static class GenreEndpoint
    {
        public static void MapGenreEndpoint(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/genres")
                .WithTags("Genres");

            group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
                [AsParameters] GetGenresParameters parameters) =>
            {
                var queryResult = await mediator.Send(new GetGenresQuery(
                    parameters.Name,
                    parameters.PageNumber,
                    parameters.PageSize), cancellationToken);

                return Results.Ok(
                    PaginatedResponse<ApplicationGenre>.FromApplicationResponse(
                        queryResult,
                        GenreResponse.FromApplicationResponse));
            })
                .WithSummary("Get Genres By Name");

            group.MapGet("/{id}", async (IMediator mediator, CancellationToken cancellationToken, int id) =>
            {
                var queryResult = await mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
                return queryResult.IsSuccess
                    ? Results.Ok(GenreResponse.FromApplicationResponse(queryResult.Value))
                    : queryResult.ToMinimalApiResult();
            })
                .WithSummary("Get Genre By Id");

            group.MapPost("/", async (IMediator mediator, CancellationToken cancellationToken, [FromBody] CreateGenreRequest request) =>
            {
                var commandResult = await mediator.Send(new CreateGenreCommand(request.Name), cancellationToken);
                return commandResult.IsSuccess
                    ? Results.Created($"/genres/{commandResult.Value.Id}", GenreResponse.FromApplicationResponse(commandResult.Value))
                    : commandResult.ToMinimalApiResult();
            })
                .WithSummary("Create Genre")
                .RequireAuthorization();

            group.MapDelete("/{id}", async (IMediator mediator, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveGenreCommand(id), cancellationToken);
                return commandResult.IsSuccess
                    ? Results.NoContent()
                    : commandResult.ToMinimalApiResult();
            })
                .WithSummary("Delete Genre")
                .RequireAuthorization();

            group.MapPut("/{id}", async (IMediator mediator, CancellationToken cancellationToken, int id, [FromBody] UpdateGenreRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateGenreCommand(id, request.Name), cancellationToken);
                return commandResult.IsSuccess
                    ? Results.Ok(GenreResponse.FromApplicationResponse(commandResult.Value))
                    : commandResult.ToMinimalApiResult();
            })
                .WithSummary("Update Genre")
                .RequireAuthorization();
        }
    }
}
