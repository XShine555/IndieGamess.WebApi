using Scalar.AspNetCore;
using WebApi.Services;

var builder = WebApplication.CreateBuilder();
var services = builder.Services;

services.AddOpenApi();
services.AddMediator();

services.AddHttpContextAccessor();
services.AddScoped<IUser, User>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();