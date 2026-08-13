using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Design-time factory so `dotnet ef migrations add` works without booting the API.
/// Reads the ConnectionStrings__Postgres env var; throws if it is not set (no secret in source).
/// </summary>
public sealed class TransactionsDbContextFactory : IDesignTimeDbContextFactory<TransactionsDbContext>
{
    public TransactionsDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? throw new InvalidOperationException(
                "Set the ConnectionStrings__Postgres environment variable before running EF Core design-time commands.");

        var options = new DbContextOptionsBuilder<TransactionsDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", TransactionsDbContext.Schema))
            .Options;

        return new TransactionsDbContext(options);
    }
}
