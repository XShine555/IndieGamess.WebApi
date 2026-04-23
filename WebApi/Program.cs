using Application.Configuration;
using Infrastructure.Messaging.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Scalar.AspNetCore;
using WebApi.Authentication;
using WebApi.Common;
using WebApi.Mappers;
using WebApi.Scalar;
using WebApi.Services;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDatabase(configuration);

services.AddProblemDetails();
services.AddValidation();

services.ConfigureAuthentication(configuration);
services.AddAuthorization();

services.AddOpenApi();
services.AddS3Service(configuration);
services.AddMassTransitClient(configuration);
services.AddApplicationMediator(configuration);

services.AddHttpContextAccessor();
services.AddScoped<ICurrentUser, CurrentUser>();
services.AddScoped<IGameApplicationMapper, GameApplicationMapper>();
services.AddScoped<IGameApplicationBuildMapper, GameBuildApplicationMapper>();
services.AddScoped<IGenreApplicationMapper, GenreApplicationMapper>();
services.AddScoped<IUserApplicationMapper, UserApplicationMapper>();
services.AddScoped<IAdminGameApplicationMapper, AdminGameApplicationMapper>();
services.AddScoped<IAdminGenreApplicationMapper, AdminGenreApplicationMapper>();
services.AddScoped<IAdminUserApplicationMapper, AdminUserApplicationMapper>();

services.AddControllers();
services.ConfigureScalar();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();

app.Run();