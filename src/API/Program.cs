using ExchangeTracing.Modules.Assets.Infrastructure;
using ExchangeTracing.Modules.Portfolio.Infrastructure;
using ExchangeTracing.Modules.Transactions.Infrastructure;
using ExchangeTracing.Modules.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register each module through its own composition entry point.
builder.Services
    .AddUsersModule(builder.Configuration)
    .AddAssetsModule(builder.Configuration)
    .AddTransactionsModule(builder.Configuration)
    .AddPortfolioModule();

var app = builder.Build();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
