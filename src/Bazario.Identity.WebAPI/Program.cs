using Bazario.AspNetCore.Shared.Auth.DependencyInjection;
using Bazario.Identity.Application;
using Bazario.Identity.Infrastructure;
using Bazario.Identity.Infrastructure.Extensions;
using Bazario.Identity.WebAPI.Extensions;
using Bazario.Identity.WebAPI.Extensions.DI;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();

var app = builder.Build();

app.Services.ValidateAppOptions();

app.AddMiddleware();

app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
} 
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.Run();