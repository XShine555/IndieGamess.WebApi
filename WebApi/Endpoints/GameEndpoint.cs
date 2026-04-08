using Application.Games.Commands;
using Application.Games.Queries;
using Application.Games.Responses;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Contracts.Games;
using WebApi.Services;

namespace WebApi.Endpoints
{
    public static class GameEndpoint
    {
        public static void MapGameEndpoint(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/games")
                .WithTags("Games");

            group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
                [AsParameters] GetGamesParameters parameters) =>
            {
                var queryResult = await mediator.Send(new GetGamesQuery(
                    parameters.Title,
                    parameters.Genres,
                    parameters.PageNumber,
                    parameters.PageSize), cancellationToken);

                return Results.Ok(
                    PaginatedResponse<ApplicationGame>.FromApplicationResponse(
                        queryResult,
                        GameResponse.FromApplicationResponse));
            } )
                .WithSummary("Get Games");

            group.MapGet("/{id}",async (IMediator mediator, CancellationToken cancellationToken, int id) =>
            {
                var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
                return queryResult.Map(GameResponse.FromApplicationResponse)
                    .ToMinimalApiResult();
            } )
                .WithSummary("Get Game By Id");

            group.MapPost("/",async (IMediator mediator, IUser user, CancellationToken cancellationToken,
                [FromBody] CreateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new CreateGameCommand(
                    user.IdentityId,
                    request.Title,
                    request.Description,
                    request.Genres), cancellationToken);
                return commandResult.Map(r => Results.Created($"/games/{r.Id}", GameResponse.FromApplicationResponse(r)))
                    .ToMinimalApiResult();
            } )
                .WithSummary("Create Game")
                .RequireAuthorization();

            group.MapDelete("/{id}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveGameCommand(id), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Delete Game")
                .RequireAuthorization();

            group.MapPut("/{id}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, int id,
                [FromBody] UpdateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateGameCommand(
                    id,
                    request.Title,
                    request.Description,
                    request.OwnerId,
                    request.Genres), cancellationToken);
                return commandResult.ToMinimalApiResult();
             } )
                .WithSummary("Update Game")
                .RequireAuthorization();
        }
    }
}