using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Portfolio.Infrastructure;

/// <summary>
/// Composition entry point for the Portfolio module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class PortfolioModuleExtensions
{
    public static IServiceCollection AddPortfolioModule(this IServiceCollection services)
    {
        // Module services (handlers, market-data adapters) will be registered here.
        return services;
    }
}
