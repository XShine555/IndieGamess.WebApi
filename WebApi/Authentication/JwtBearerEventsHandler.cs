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
            var username = tokenValidatedContext.Principal!.FindFirstValue("username")!;
            await mediator.Send(new CreateUserCommand(Guid.Parse(id), username), tokenValidatedContext.HttpContext.RequestAborted);
        }
    }
}