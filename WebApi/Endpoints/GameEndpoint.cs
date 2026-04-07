using Application.Games.Commands;
using Application.Games.Queries;
using Application.Games.Responses;
using Ardalis.Result;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Contracts.Games;
using WebApi.Services;

namespace WebApi.Endpoints
{
    public class GameEndpoint
    {
        public static void Map(WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/games")
                .WithTags("Games");

            group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
                [FromQuery] string title, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
            {
                var queryResult = await mediator.Send(new GetGamesByTitleQuery(title, pageNumber, pageSize), cancellationToken);
                return Results.Ok(
                    PaginatedResponse<ApplicationGame>.FromApplicationResponse(
                        queryResult,
                        GameResponse.FromApplicationResponse));
            } )
                .WithSummary("Get Games By Title");

            group.MapGet("/genres",async (IMediator mediator, CancellationToken cancellationToken,
                [FromQuery] Guid[] genreIds, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
            {
                var queryResult = await mediator.Send(new GetGamesByGenresQuery(genreIds, pageNumber, pageSize), cancellationToken);
                return Results.Ok(
                    PaginatedResponse<ApplicationGame>.FromApplicationResponse(
                        queryResult,
                        GameResponse.FromApplicationResponse));
            } )
                .WithSummary("Get Games By Genres");

            group.MapGet("/{id:guid}",async (IMediator mediator, CancellationToken cancellationToken, Guid id) =>
            {
                var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);
                return queryResult.Map(GameResponse.FromApplicationResponse);
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
                return commandResult.Map(r => Results.Created($"/games/{r.Id}", GameResponse.FromApplicationResponse(r)));
            } )
                .WithSummary("Create Game")
                .RequireAuthorization();

            group.MapDelete("/{id:guid}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, Guid id) =>
            {
                var commandResult = await mediator.Send(new RemoveGameCommand(id), cancellationToken);
                return commandResult;
            } )
                .WithSummary("Delete Game")
                .RequireAuthorization();

            group.MapPut("/{id:guid}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, Guid id,
                [FromBody] UpdateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateGameCommand(
                    id,
                    request.Title,
                    request.Description,
                    request.OwnerId,
                    request.Genres), cancellationToken);
                return commandResult;
             } )
                .WithSummary("Update Game")
                .RequireAuthorization();
        }
    }
}