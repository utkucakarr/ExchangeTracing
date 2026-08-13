using ExchangeTracing.Modules.Assets.Infrastructure;
using ExchangeTracing.Modules.Portfolio.Infrastructure;
using ExchangeTracing.Modules.Transactions.Infrastructure;
using ExchangeTracing.Modules.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register each module through its own composition entry point.
builder.Services
    .AddUsersModule()
    .AddAssetsModule()
    .AddTransactionsModule()
    .AddPortfolioModule();

var app = builder.Build();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
