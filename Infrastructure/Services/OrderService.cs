using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRespo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        // Since IBasketRepository belongs to Identity.db and NOT skinet.db
        // we'll keep it as another injection
        public OrderService(IBasketRepository basketRespo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _basketRespo = basketRespo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippToAddress)
        {
            // get the basket from basketRepo and check the order price and quantity and compared the price & quantity from database
            var basket = await _basketRespo.GetBasketAsync(basketId);

            // create an instance of list of OrderItems
            List<OrderItem> listOfOrderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                listOfOrderItems.Add(orderItem);
            }

            // get the delivery method by the PASSED-IN deliveryMethodId
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calculate subtotal of ALL the ListOfOrderItems
            var subtotal = listOfOrderItems.Sum(item => item.Price * item.Quantity);

            // Get an Order by paymentIntentId using OrderByPaymentIntentWithItemsSpecification class’ constructor
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);

            // Go to the Repository and get the Order that has this paymentIntentId
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            // check if an Order exists
            if (existingOrder != null) 
            {
                // Order existed, then DELETE this Order
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }

            // create this Order using the Order parametered CONSTRUCTOR with this parameters
            var order = new Order(listOfOrderItems, buyerEmail, shippToAddress, deliveryMethod, subtotal, basket.PaymentIntentId);

            // add the Order into the Repository using Unit of Work
            _unitOfWork.Repository<Order>().Add(order);

            // save the order to database by calling the Unit of Work COMPLETE method
            int result = await _unitOfWork.Complete();

            // if the RETURN result is a NEGATIVE integer number; means FAILED to SAVE to Database
            // then return NULL
            if (result <= 0)
            {
                return null;
            }

            // return the order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var listOfDeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();

            return listOfDeliveryMethods;
        }

         public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);

            var orderById = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            return orderById;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            var listOfUserOrders = await _unitOfWork.Repository<Order>().ListAsync(spec);

            return listOfUserOrders;
        }

    }
}









// namespace Infrastructure.Services
// {
//      public class OrderService : IOrderService
//     {
//         private readonly IGenericRepository<Order> _orderRepo;
//         private readonly IGenericRepository<Product> _productRepo;
//         private readonly IBasketRepository _basketRespo;  
//         private readonly IGenericRepository<DeliveryMethod> _dmRepo;

//         public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> dmRepo, IGenericRepository<Product> productRepo, IBasketRepository basketRespo)
//         {
//             _basketRespo = basketRespo;
//             _productRepo = productRepo;
//             _orderRepo = orderRepo;
//             _dmRepo = dmRepo;            
//         }

//         public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
//         {
//             // get the basket from basketRepo and check the order price and quantity and compared the price & quantity from database
//             var basket = await _basketRespo.GetBasketAsync(basketId);

//             // Instantiate a new List of OrderItem
//             List<OrderItem> listofOrderItems = new List<OrderItem>();

//             // get the items from the ProductRepository
//             foreach (var item in basket.Items)
//             {
//                 var productItem = await _productRepo.GetByIdAsync(item.Id);
//                 var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
//                 var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

//                 listofOrderItems.Add(orderItem);
//             }

//             // get the delivery method from DeliveryMethodRepository
//             var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);

//             // calculate subtotal
//             var subtotal = listofOrderItems.Sum(item => item.Price * item.Quantity);

//             // create order (NOTE: the order of parameters are MUST MATCHED the parameters set in Order class)
//             var order = new Order(listofOrderItems, buyerEmail, shippingAddress, deliveryMethod, subtotal);

//             // TODO: save the order to database

//             // return the order
//             return order;
//         }

//         public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
//         {
//             throw new NotImplementedException();
//         }

//         public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }








// namespace Infrastructure.Services
// {
//      public class OrderService : IOrderService
//     {
//         public Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
//         {
//             throw new NotImplementedException();

//             // ***** TASKS we needed to do to Create an Order ********

//             // Get Basket from the BasketRepository
//             // Get Items from the ProductRepository
//             // Get a Delivery Method from DeliveryMethodRepository
//             // Calculate the Subtotal
//             // Save the Order to Database
//             // Return the order
//         }

//         public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
//         {
//             throw new NotImplementedException();
//         }

//         public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }









