using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Persistence boundary for the Transactions module. Lives in its own PostgreSQL schema
/// inside the single shared database. Entities are added in later feature steps.
/// </summary>
public sealed class TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : DbContext(options)
{
    public const string Schema = "transactions";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }
}
