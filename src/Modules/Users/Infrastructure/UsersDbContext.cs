using ExchangeTracing.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Users.Infrastructure;

/// <summary>
/// Persistence boundary for the Users module. Lives in its own PostgreSQL schema
/// inside the single shared database.
/// </summary>
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public const string Schema = "users";

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
