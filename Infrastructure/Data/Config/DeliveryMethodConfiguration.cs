using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Configuring DeliveryMthod class model
    /// </summary>
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            // Telling EF that the PRICE property is in DECIMAL data type and formated
            builder.Property(d => d.Price).HasColumnType("decimal(18,2)");
        }
    }
}