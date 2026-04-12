using Application.Games.Commands;
using Application.Games.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Services;

namespace WebApi.Features.Games
{
    public static class GameEndpoint
    {
        public static void MapGameEndpoint(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/games")
                .WithTags("Games");

            group.MapGet("/", async (IMediator mediator, GameResponseMapper mapper, CancellationToken cancellationToken,
                [AsParameters] GetGamesParameters parameters) =>
            {
                var queryResult = await mediator.Send(new GetGamesQuery(
                    parameters.Title,
                    parameters.Genres,
                    parameters.PageNumber,
                    parameters.PageSize), cancellationToken);

                var response = await PaginatedResponse<GameResponse>.FromApplicationResponseAsync(
                    queryResult,
                    game => mapper.FromApplicationResponseAsync(game, cancellationToken),
                    cancellationToken);

                return Results.Ok(response);
            } )
                .WithSummary("Get Games");

            group.MapGet("/{id}", async (IMediator mediator, GameResponseMapper mapper, CancellationToken cancellationToken, int id) =>
            {
                var queryResult = await mediator.Send(new GetGameByIdQuery(id), cancellationToken);

                if (!queryResult.IsSuccess)
                {
                    return queryResult.ToMinimalApiResult();
                }

                var response = await mapper.FromApplicationResponseAsync(queryResult.Value, cancellationToken);
                return Results.Ok(response);
            } )
                .WithSummary("Get Game By Id");

            group.MapPost("/", async (IMediator mediator, ICurrentUser currentUser, GameResponseMapper mapper, CancellationToken cancellationToken,
                [FromBody] CreateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new CreateGameCommand(
                    currentUser.IdentityId,
                    request.Title,
                    request.Description,
                    request.Genres), cancellationToken);

                if (!commandResult.IsSuccess)
                {
                    return commandResult.ToMinimalApiResult();
                }

                var response = await mapper.FromApplicationResponseAsync(commandResult.Value, cancellationToken);
                return Results.Created($"/games/{commandResult.Value.Id}", response);
            } )
                .WithSummary("Create Game")
                .RequireAuthorization();

            group.MapPost("/{id}/picture", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken,
                int id, [FromForm] IFormFile formFile) =>
            {
                var fileData = FileData.FromFormFile(formFile);
                var commandResult = await mediator.Send(new AddStorePictureToGameCommand(currentUser.IdentityId, id, fileData), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Upload Game Picture")
                .RequireAuthorization()
                .DisableAntiforgery();

            group.MapDelete("/{id}/picture", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveStorePictureToGameCommand(currentUser.IdentityId, id), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Delete Game Picture")
                .RequireAuthorization()
                .DisableAntiforgery();

            group.MapDelete("/{id}", async (IMediator mediator, ICurrentUser currentUser, CancellationToken cancellationToken, int id) =>
            {
                var commandResult = await mediator.Send(new RemoveGameCommand(currentUser.IdentityId, id), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Delete Game")
                .RequireAuthorization();

            group.MapPut("/{id}", async (IMediator mediator, ICurrentUser currentUser, GameResponseMapper mapper, CancellationToken cancellationToken, int id,
                [FromBody] UpdateGameRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateGameCommand(
                    currentUser.IdentityId,
                    id,
                    request.Title,
                    request.Description,
                    request.OwnerId,
                    request.Genres), cancellationToken);

                if (!commandResult.IsSuccess)
                {
                    return commandResult.ToMinimalApiResult();
                }

                var response = await mapper.FromApplicationResponseAsync(commandResult.Value, cancellationToken);
                return Results.Ok(response);
            } )
                .WithSummary("Update Game")
                .RequireAuthorization();
        }
    }
}
