using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Composition entry point for the Users module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        // Module services (DbContext, handlers, repositories) will be registered here.
        return services;
    }
}
