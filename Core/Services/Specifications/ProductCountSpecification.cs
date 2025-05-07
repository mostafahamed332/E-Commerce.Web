﻿using Domain.Models.Products;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
	internal class ProductCountSpecification : BaseSpecifications<Product, int>
	{
		public ProductCountSpecification(ProductQueryParams productQuery)
			: base(p =>
				(!productQuery.BrandId.HasValue || p.BrandId == productQuery.BrandId) &&
				(!productQuery.TypeId.HasValue || p.TypeId == productQuery.TypeId) &&
				(string.IsNullOrEmpty(productQuery.SearchValue) ||
				 p.Name.ToLower().Contains(productQuery.SearchValue.ToLower()))
			)
		{
		}
		
	}
}
