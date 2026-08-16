using ExchangeTracing.Modules.Assets.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

/// <summary>
/// Composition entry point for the Assets module. The API host calls this to
/// register the module's services without knowing its internals.
/// </summary>
public static class AssetsModuleExtensions
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AssetsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", AssetsDbContext.Schema)));

        services.AddScoped<IAssetRepository, AssetRepository>();

        var applicationAssembly = typeof(AssetDto).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddHealthChecks()
            .AddDbContextCheck<AssetsDbContext>("assets-db");

        return services;
    }
}
