using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    /// <summary>
    /// ProductItemOrdered class model represent the snapshot of the Order
    /// at the time of the order is processed
    /// </summary>
    public class ProductItemOrdered
    {
        // parameterless constructor
        public ProductItemOrdered()
        {            
        }

        // parametered constructor
        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}