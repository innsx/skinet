
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<IPaymentService> _logger;
        private const string StripeWebhooksSecret = "whsec_ea8a7e49cf0e45bb6a69e35e79b1cad264ab4c670257300709b078f946c0853a";
        public PaymentsController(IPaymentService paymentService, ILogger<IPaymentService> logger)
        {
            _logger = logger;
            _paymentService = paymentService;            
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null)
            {
                return BadRequest(new ApiResponse(400, "Problem with Creating/Updating your Basket."));
            }
            
            return basket;
        }

        [HttpPost("webhooks")]
        public async Task<ActionResult> StripeWebHook() 
        {
            // read responses from STRIPE
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // create a STRIPE Event using STRIPE Response in JSON format
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], StripeWebhooksSecret);

            PaymentIntent paymentIntent;
            Core.Entities.OrderAggregate.Order order;

            switch(stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded ", paymentIntent.Id);

                    // Update the order status to paid or shipped based on your business logic
                    order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Payment updated to payment received: ", order.Id);
                    break;

                case "payment_intent.payment_failed":
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogError("Payment Failed", paymentIntent.Id);

                    // Update the order status to failed based on your business logic
                    order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Payment Failed: ", order.Id);
                    break;

                default:
                    _logger.LogInformation("Something went wrong with the Payment Intent Event! ");
                    break;                    
            }

            // confirm to STRIPE with an EmptyResult object
            return new EmptyResult();
        }
    }    
}
