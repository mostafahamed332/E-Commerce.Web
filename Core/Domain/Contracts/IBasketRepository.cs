﻿using Domain.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetBasketAsync(string key);

		Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket customerBasket, TimeSpan? TimeToLive = null);

		Task<bool> DeleteBasketAsync(string key);
	}
}
