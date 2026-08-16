using ExchangeTracing.Api;
using ExchangeTracing.BuildingBlocks.Behaviors;
using ExchangeTracing.Modules.Assets.Infrastructure;
using ExchangeTracing.Modules.Portfolio.Infrastructure;
using ExchangeTracing.Modules.Transactions.Infrastructure;
using ExchangeTracing.Modules.Users.Infrastructure;
using ExchangeTracing.Modules.Users.Presentation;
using ExchangeTracing.Modules.Assets.Presentation;
using MediatR;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(UsersController).Assembly)
    .AddApplicationPart(typeof(AssetsController).Assembly);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Register each module through its own composition entry point.
builder.Services
    .AddUsersModule(builder.Configuration)
    .AddAssetsModule(builder.Configuration)
    .AddTransactionsModule(builder.Configuration)
    .AddPortfolioModule();

// Shared MediatR validation pipeline, registered once for every module.
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                 // OpenAPI document at /openapi/v1.json
    app.MapScalarApiReference();      // Scalar UI at /scalar/v1
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
