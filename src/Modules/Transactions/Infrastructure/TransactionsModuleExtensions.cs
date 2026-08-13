using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Composition entry point for the Transactions module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class TransactionsModuleExtensions
{
    public static IServiceCollection AddTransactionsModule(this IServiceCollection services)
    {
        // Module services (DbContext, handlers, repositories) will be registered here.
        return services;
    }
}
