using Bazario.AspNetCore.Shared.Authentication.DependencyInjection;
using Bazario.AspNetCore.Shared.Authorization.DependencyInjection;
using Bazario.Identity.Application;
using Bazario.Identity.Infrastructure;
using Bazario.Identity.Infrastructure.Extensions;
using Bazario.Identity.WebAPI.Extensions;
using Bazario.Identity.WebAPI.Extensions.DI;
using Bazario.Identity.WebAPI.Factories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddPresentationOptions();

builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();

builder.Services.AddSingleton<ProblemDetailsFactory>();

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

app.MapControllers();

app.SeedRoles().Wait();
app.SeedOwnerIfNotSeeded().Wait();

app.Run();