using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Design-time factory so `dotnet ef migrations add` works without booting the API.
/// Reads the ConnectionStrings__Postgres env var; throws if it is not set (no secret in source).
/// </summary>
public sealed class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? throw new InvalidOperationException(
                "Set the ConnectionStrings__Postgres environment variable before running EF Core design-time commands.");

        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", UsersDbContext.Schema))
            .Options;

        return new UsersDbContext(options);
    }
}
