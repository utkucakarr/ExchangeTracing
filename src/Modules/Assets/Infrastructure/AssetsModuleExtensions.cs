using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

/// <summary>
/// Composition entry point for the Assets module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class AssetsModuleExtensions
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services)
    {
        // Module services (DbContext, handlers, repositories) will be registered here.
        return services;
    }
}
