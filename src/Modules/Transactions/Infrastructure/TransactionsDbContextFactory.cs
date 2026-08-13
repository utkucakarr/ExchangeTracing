using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Design-time factory so `dotnet ef migrations add` works without booting the API.
/// Uses the ConnectionStrings__Postgres env var, falling back to the local dev database.
/// </summary>
public sealed class TransactionsDbContextFactory : IDesignTimeDbContextFactory<TransactionsDbContext>
{
    public TransactionsDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? "Host=localhost;Port=5432;Database=exchangetracing;Username=exchangetracing;Password=exchangetracing";

        var options = new DbContextOptionsBuilder<TransactionsDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", TransactionsDbContext.Schema))
            .Options;

        return new TransactionsDbContext(options);
    }
}
