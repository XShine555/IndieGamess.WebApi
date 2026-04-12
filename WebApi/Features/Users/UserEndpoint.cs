using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi.Features.Users
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoint(this WebApplication webApplication)
        {
            var group = webApplication.MapGroup("/users")
                .WithTags("Users");

            group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken, UserResponseMapper mapper,
                [AsParameters] GetUsersParameters parameters) =>
            {
                var queryResult = await mediator.Send(new GetUsersQuery(parameters.Name, parameters.PageNumber, parameters.PageSize), cancellationToken);

                var response = await PaginatedResponse<UserResponse>.FromApplicationResponseAsync(
                    queryResult,
                    user => mapper.FromApplicationResponseAsync(user, cancellationToken),
                    cancellationToken);

                return Results.Ok(response);
            } )
                .WithSummary("Get Users By Display Username");

            group.MapGet("/{id}", async (IMediator mediator, UserResponseMapper mapper, CancellationToken cancellationToken, string id) =>
            {
                var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);

                if (!queryResult.IsSuccess)
                {
                    return queryResult.ToMinimalApiResult();
                }

                var response = await mapper.FromApplicationResponseAsync(queryResult.Value, cancellationToken);
                return Results.Ok(response);
            } )
                .WithSummary("Get User By Id");

            group.MapPost("/{id}/image", async (IMediator mediator, CancellationToken cancellationToken,
                string id, [FromForm] IFormFile formFile) =>
            {
                var fileData = FileData.FromFormFile(formFile);
                var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(id, fileData), cancellationToken);
                return commandResult.ToMinimalApiResult();
            } )
                .WithSummary("Upload User Profile Image")
                .RequireAuthorization()
                .DisableAntiforgery();

            group.MapPut("/{id}", async (IMediator mediator, UserResponseMapper mapper, CancellationToken cancellationToken,
                string id, [FromBody] UpdateUserRequest request) =>
            {
                var commandResult = await mediator.Send(new UpdateUserCommand(id, request.DisplayUsername), cancellationToken);

                if (!commandResult.IsSuccess)
                {
                    return commandResult.ToMinimalApiResult();
                }

                var response = await mapper.FromApplicationResponseAsync(commandResult.Value, cancellationToken);
                return Results.Ok(response);
            } )
                .WithSummary("Update User")
                .RequireAuthorization();
        }
    }
}
