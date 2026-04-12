using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Features.Users;

public static class UserEndpoint
{
    public static void MapUserEndpoint(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("/users")
            .WithTags("Users");

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
            [AsParameters] GetUsersParameters parameters) =>
        {
            var queryResult = await mediator.Send(new GetUserByDisplayUsernameQuery(parameters.Name), cancellationToken);

            if (queryResult.IsSuccess)
            {
                var summary = UserSummary.FromApplicationResponse(queryResult.Value);
                return Results.Ok(new PaginatedResponse<UserSummary>(
                    [summary],
                    1,
                    1,
                    1,
                    1,
                    false,
                    false));
            }

            if (queryResult.Status == ResultStatus.NotFound)
            {
                return Results.Ok(new PaginatedResponse<UserSummary>(
                    [],
                    1,
                    10,
                    0,
                    0,
                    false,
                    false));
            }

            return queryResult.ToMinimalApiResult();
        })
            .WithSummary("Get Users By Display Username");

        group.MapGet("/{id}", async (IMediator mediator, CancellationToken cancellationToken, string id) =>
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);

            return queryResult.IsSuccess
                ? Results.Ok(UserResponse.FromApplicationResponse(queryResult.Value))
                : queryResult.ToMinimalApiResult();
        })
            .WithSummary("Get User By Id");

        group.MapPost("/{id}/image", async (IMediator mediator, CancellationToken cancellationToken,
            string id, [FromForm] IFormFile formFile) =>
        {
            var fileData = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(id, fileData), cancellationToken);
            return commandResult.ToMinimalApiResult();
        })
            .WithSummary("Upload User Profile Image")
            .RequireAuthorization()
            .DisableAntiforgery();

        group.MapPut("/{id}", async (IMediator mediator, CancellationToken cancellationToken,
            string id, [FromBody] UpdateUserRequest request) =>
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(id, request.DisplayUsername), cancellationToken);
            return commandResult.IsSuccess
                ? Results.Ok(UserResponse.FromApplicationResponse(commandResult.Value))
                : commandResult.ToMinimalApiResult();
        })
            .WithSummary("Update User")
            .RequireAuthorization();
    }
}
