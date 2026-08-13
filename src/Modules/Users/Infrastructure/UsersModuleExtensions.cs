using ExchangeTracing.Modules.Users.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Composition entry point for the Users module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class UsersModuleExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", UsersDbContext.Schema)));

        services.AddScoped<IUserRepository, UserRepository>();

        var applicationAssembly = typeof(UserDto).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddHealthChecks()
            .AddDbContextCheck<UsersDbContext>("users-db");

        return services;
    }
}
