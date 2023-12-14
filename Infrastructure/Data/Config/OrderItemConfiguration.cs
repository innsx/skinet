using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /// <summary>
    /// Configuring OrderItem class model
    /// </summary>
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered, io => {
                                                        // Configuring the MODEL as a whole
                                                        io.WithOwner();

                                                        // OR Configuring Each Property
                                                        // io.Property(p => p.ProductItemId);
                                                        // io.Property(p => p.ProductName);
                                                        // io.Property(p => p.PictureUrl);                                                        
                                                      });

            // Telling EF that the PRICE property is in DECIMAL data type and formated
            builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
        }
    }
}
