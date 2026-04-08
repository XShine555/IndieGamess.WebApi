using Application;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Scalar.AspNetCore;
using WebApi.Authentication;
using WebApi.Endpoints;
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
ServiceDependencyInjection.AddS3Service(services, configuration);
services.AddApplicationConfiguration(configuration);
services.AddMediator();

services.AddHttpContextAccessor();
services.AddScoped<IUser, User>();

services.ConfigureScalar();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGameEndpoint();
app.MapGenreEndpoint();

app.Run();