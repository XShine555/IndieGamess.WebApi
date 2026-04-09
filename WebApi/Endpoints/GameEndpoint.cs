using Application.Games.Commands;
using Application.Games.Queries;
using Application.Users.Queries;
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

                return Results.Ok(PaginatedResponse<GameResponse>.FromApplicationResponse(
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

                return commandResult.Map(r => Results.Created($"/games/{commandResult.Value.Id}", GameResponse.FromApplicationResponse(r)))
                        .ToMinimalApiResult();
            } )
                .WithSummary("Create Game")
                .RequireAuthorization();

            group.MapPost("/{id}/picture",async (IMediator mediator, IUser user, CancellationToken cancellationToken,
                int id, [FromForm] IFormFile formFile) =>
            {
                var fileData = FileData.FromFormFile(formFile);
                var commandResult = await mediator.Send(new AddStorePictureToGameCommand(user.IdentityId, id, fileData), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Upload Game Picture")
                .RequireAuthorization()
                .DisableAntiforgery();

            group.MapDelete("/{id}/picture",async (IMediator mediator, IUser user, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(user.IdentityId, id), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Delete Game Picture")
                .RequireAuthorization()
                .DisableAntiforgery();

            group.MapDelete("/{id}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveGameCommand(user.IdentityId, id), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Delete Game")
                .RequireAuthorization();

            group.MapPut("/{id}",async (IMediator mediator, IUser user, CancellationToken cancellationToken, int id,
                [FromBody] UpdateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateGameCommand(
                    user.IdentityId,
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