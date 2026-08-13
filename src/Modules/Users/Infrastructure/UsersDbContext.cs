using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Persistence boundary for the Users module. Lives in its own PostgreSQL schema
/// inside the single shared database. Entities are added in later feature steps.
/// </summary>
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public const string Schema = "users";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }
}
