using Domain.Contracts;
using Domain.Models.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
	{
		private readonly IDatabase _database = connection.GetDatabase();

		public async Task<CustomerBasket?> GetBasketAsync(string key)
		{
			var Basket = await _database.StringGetAsync(key);

			if (string.IsNullOrEmpty(Basket)) return null;

			else 
				return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
		}

		public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket customerBasket, TimeSpan? TimeToLive = null)
		{
			var JsonBasket = JsonSerializer.Serialize(customerBasket);

			var IsCreatedOrUpdated = await _database.StringSetAsync(customerBasket.Id, JsonBasket, TimeToLive?? TimeSpan.FromDays(30));

			if (IsCreatedOrUpdated)
				return await GetBasketAsync(customerBasket.Id);

			else
				return null;


		}

		public async Task<bool> DeleteBasketAsync(string key)
		{
			return await _database.KeyDeleteAsync(key);	
		}

		
	}
}
