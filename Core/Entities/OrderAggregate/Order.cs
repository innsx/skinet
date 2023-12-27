using System;
using System.Collections.Generic;

namespace Core.Entities.OrderAggregate
{
    /// <summary>
    /// Order class model represented the AGGREGaTe 
    /// of these classes: Address, DeliveryMethod, ProductItemOrdered
    /// OrderStatus, OrderItem
    /// </summary>
    public class Order : BaseEntity
    {
        // parameterless constructor when this class is Instantiated WITHOUT parameters
        public Order()
        {
        }

        // parametered constructor when this class is Instantiated WITH parameters
        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress, 
                     DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            OrderItems = orderItems;
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;            
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }

        // OrderDate property with an initialized value
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; 

        public Address ShipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        public decimal Subtotal { get; set; }

        // Status property with an initialized value to Pending
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // a string Id which Identified a UNIQUE STRIDE payment
        public string PaymentIntentId { get; set; }


        // a method that AUTOMATIC executed when this Order class is Instantiated
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
