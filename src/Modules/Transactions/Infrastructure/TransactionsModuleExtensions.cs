using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Composition entry point for the Transactions module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class TransactionsModuleExtensions
{
    public static IServiceCollection AddTransactionsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", TransactionsDbContext.Schema)));

        services.AddHealthChecks()
            .AddDbContextCheck<TransactionsDbContext>("transactions-db");

        return services;
    }
}
