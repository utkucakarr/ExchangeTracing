using ExchangeTracing.Modules.Transactions.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

/// <summary>
/// Persistence boundary for the Transactions module. Lives in its own PostgreSQL schema
/// inside the single shared database.
/// </summary>
public sealed class TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : DbContext(options)
{
    public const string Schema = "transactions";

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
