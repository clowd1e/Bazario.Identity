using Bazario.AspNetCore.Shared.Api.Factories.DependencyInjection;
using Bazario.AspNetCore.Shared.Api.Logging.DependencyInjection;
using Bazario.AspNetCore.Shared.Api.Middleware.DependencyInjection;
using Bazario.AspNetCore.Shared.Authentication.DependencyInjection;
using Bazario.AspNetCore.Shared.Authorization.DependencyInjection;
using Bazario.Identity.Application;
using Bazario.Identity.Infrastructure;
using Bazario.Identity.Infrastructure.Extensions;
using Bazario.Identity.WebAPI.Extensions;
using Bazario.Identity.WebAPI.Extensions.DI;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddPresentationOptions();

builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();

builder.Services.AddProblemDetailsFactory();

builder.Host.ConfigureSerilogFromConfiguration();

var app = builder.Build();

app.Services.ValidateAppOptions();

app.UseExceptionHandlingMiddleware();

app.UseRequestLogContextMiddleware();

app.UseSerilogRequestLogging();

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