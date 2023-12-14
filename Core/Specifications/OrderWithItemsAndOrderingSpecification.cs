using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        // get ALL orders by userEmail
        public OrderWithItemsAndOrderingSpecification(string userEmail) : base(o => o.BuyerEmail == userEmail) // using base to Specifiy the criteria
        {
            // EAGER LOADING OrderItems & DeliveryMethod entities
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);

            // SORT BY ORDERDATE IN DESCENDING
            AddOrderByDescending(o => o.OrderDate);
        }

        // CONSTRUCTOR WITH INDIVIDUAL ORDER BASED ON buyer's id & email
        public OrderWithItemsAndOrderingSpecification(int id, string userEmail) : base(o => o.Id == id && o.BuyerEmail == userEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
