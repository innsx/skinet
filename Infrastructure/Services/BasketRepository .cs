using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redisDatabase;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();            
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            bool isBasketIdDeleted = await _redisDatabase.KeyDeleteAsync(basketId);

            if (isBasketIdDeleted == false)
            {
                return false;
            }

            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _redisDatabase.StringGetAsync(basketId);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            var deserializedCustomerBasketData = JsonSerializer.Deserialize<CustomerBasket>(data);

            return deserializedCustomerBasketData;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            string CustomerBasketId = basket.Id;
            string serializedJsonBasket = JsonSerializer.Serialize(basket);
            var numberOfDaysBasketExpired = TimeSpan.FromDays(30);

            bool isCreateBasket = await _redisDatabase.StringSetAsync(CustomerBasketId, serializedJsonBasket, numberOfDaysBasketExpired);

            if (isCreateBasket == false)
            {
                return null;
            }

            var basketId = await GetBasketAsync(basket.Id);

            return basketId;
        }
    }
}
