using Application.Users.Commands;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace WebApi.Authentication
{
    public class JwtBearerEventsHandler(IMediator mediator)
        : JwtBearerEvents
    {
        public override async Task TokenValidated(TokenValidatedContext tokenValidatedContext)
        {
            var id = tokenValidatedContext.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var username = tokenValidatedContext.Principal!.FindFirstValue(ClaimTypes.Name)!;
            await mediator.Send(new CreateUserCommand(id, username), tokenValidatedContext.HttpContext.RequestAborted);
        }
    }
}