using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Assets.Infrastructure;

/// <summary>
/// Persistence boundary for the Assets module. Lives in its own PostgreSQL schema
/// inside the single shared database. Entities are added in later feature steps.
/// </summary>
public sealed class AssetsDbContext(DbContextOptions<AssetsDbContext> options) : DbContext(options)
{
    public const string Schema = "assets";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }
}
