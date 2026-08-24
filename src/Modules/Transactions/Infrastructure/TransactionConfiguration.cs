using ExchangeTracing.Modules.Transactions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeTracing.Modules.Transactions.Infrastructure;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.AssetId).IsRequired();
        builder.Property(t => t.Type).IsRequired();

        // decimal precision for financial values (never float/double).
        builder.Property(t => t.Quantity).HasPrecision(18, 8).IsRequired();
        builder.Property(t => t.Price).HasPrecision(18, 8).IsRequired();

        builder.Property(t => t.ExecutedAt).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();

        builder.HasIndex(t => new { t.UserId, t.AssetId });
        builder.HasIndex(t => new { t.UserId, t.ExecutedAt }).IsDescending(false, true);
    }
}
