using ConversionPath.Domain.ExchangeRates.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConversionPath.Persistence.ExchangeRateAggregate.EntityConfigs
{
    public class ExchangeRateEntityConfigs : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable("ExchangeRates", "dbo");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.HasIndex(r => r.DateTime);
            builder.HasIndex(r => new { r.SourceCurrency, r.DestinationCurrency });
            builder.HasIndex(r => new { r.SourceCurrency, r.DestinationCurrency, r.DateTime }).IsUnique();
            builder.Property(r => r.SourceCurrency).HasMaxLength(6);
            builder.Property(r => r.DestinationCurrency).HasMaxLength(6); 
        }
    }
}