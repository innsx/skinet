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
    /// In OrderConfiguration class, we're telling Entity Framework
    /// about the PROPERTIES that the Order class model owns
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // tells EF about our Address properties
            builder.OwnsOne(o => o.ShipToAddress, 
                                 a => 
                                 {
                                    // setup the whole model
                                    a.WithOwner();    

                                    // OR we can setup EACH property like this
                                    // a.Property(p => p.FirstName).IsRequired();   
                                    // a.Property(p => p.LastName).IsRequired();  
                                    // a.Property(p => p.Street).IsRequired();  
                                    // a.Property(p => p.City).IsRequired();  
                                    // a.Property(p => p.State).IsRequired();  
                                    // a.Property(p => p.Zipcode).IsRequired();                  
                                 });

            // tells EF about our 2nd property to get the ENUM STrING value
            // rather than the INTEGER value
            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o) // parsing ENUM
            );

            // tells EF about our 3rd property that When DELETING an Order,
            // the DELETING process should have a CASCADE EFFECT
            // on DELETING any OBJECT related to this DELETE Order at the same
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
