using Application.Games.Commands;
using Application.Games.Queries;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Services;

namespace WebApi.Features.Games;

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
        })
            .WithSummary("Get Games");

        group.MapGet("/{id}", async (IMediator mediator, CancellationToken cancellationToken, int id) =>
        {
            var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);

            return queryResult.IsSuccess
                ? Results.Ok(GameResponse.FromApplicationResponse(queryResult.Value))
                : queryResult.ToMinimalApiResult();
        })
            .WithSummary("Get Game By Id");

        group.MapPost("/", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken,
            [FromBody] CreateGameRequest request) =>
        {
            var commandResult = await mediator.Send(new CreateGameCommand(
                currentUser.IdentityId,
                request.Title,
                request.Description,
                request.Genres), cancellationToken);

            return commandResult.IsSuccess
                ? Results.Created($"/games/{commandResult.Value.Id}", GameResponse.FromApplicationResponse(commandResult.Value))
                : commandResult.ToMinimalApiResult();
        })
            .WithSummary("Create Game")
            .RequireAuthorization();

        group.MapPost("/{id}/picture", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken,
            int id, [FromForm] IFormFile formFile) =>
        {
            var fileData = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, fileData), cancellationToken);
            return commandResult.ToMinimalApiResult();
        })
            .WithSummary("Upload Game Picture")
            .RequireAuthorization()
            .DisableAntiforgery();

        group.MapDelete("/{id}/picture", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken, int id) =>
        {
            var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToMinimalApiResult();
        })
            .WithSummary("Delete Game Picture")
            .RequireAuthorization()
            .DisableAntiforgery();

        group.MapDelete("/{id}", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken, int id) =>
        {
            var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult.ToMinimalApiResult();
        })
            .WithSummary("Delete Game")
            .RequireAuthorization();

        group.MapPut("/{id}", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken, int id,
            [FromBody] UpdateGameRequest request) =>
        {
            var commandResult = await mediator.Send(new UpdateGameCommand(
                currentUser.IdentityId,
                id,
                request.Title,
                request.Description,
                request.OwnerId,
                request.Genres), cancellationToken);

            return commandResult.IsSuccess
                ? Results.Ok(GameResponse.FromApplicationResponse(commandResult.Value))
                : commandResult.ToMinimalApiResult();
        })
            .WithSummary("Update Game")
            .RequireAuthorization();
    }
}
