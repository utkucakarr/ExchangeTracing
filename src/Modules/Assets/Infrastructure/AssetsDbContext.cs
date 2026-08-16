using ExchangeTracing.Modules.Assets.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

/// <summary>
/// Persistence boundary for the Assets module. Lives in its own PostgreSQL schema
/// inside the single shared database.
/// </summary>
public sealed class AssetsDbContext(DbContextOptions<AssetsDbContext> options) : DbContext(options)
{
    public const string Schema = "assets";

    public DbSet<Asset> Assets => Set<Asset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
