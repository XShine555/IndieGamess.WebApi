using Application.Users.Commands;
using Ardalis.Result;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace WebApi.Authentication
{
    public class JwtBearerEventsHandler(IMediator mediator, ILogger<JwtBearerEventsHandler> logger)
        : JwtBearerEvents
    {
        public override async Task TokenValidated(TokenValidatedContext tokenValidatedContext)
        {
            var id = tokenValidatedContext.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var username = tokenValidatedContext.Principal!.FindFirstValue("username")!;

            var createResult = await mediator.Send(new CreateUserCommand(id, username), tokenValidatedContext.HttpContext.RequestAborted);
            if (!createResult.IsConflict())
                return;

            logger.LogInformation("User with id {Id} already exists, syncing user", id);
            await mediator.Send(new UpdateUserCommand(id, username), tokenValidatedContext.HttpContext.RequestAborted);
        }
    }
}