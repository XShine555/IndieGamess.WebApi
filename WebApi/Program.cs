using Scalar.AspNetCore;
using WebApi.Authentication;
using WebApi.Scalar;
using WebApi.Services;
using Infrastructure.Persistence;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDatabase(configuration);

services.AddOpenApi();
services.AddMediator();
services.ConfigureAuthentication(configuration);

services.AddHttpContextAccessor();
services.AddScoped<IUser, User>();

services.ConfigureScalar();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGameEndpoint();
app.MapGenreEndpoint();

app.Run();