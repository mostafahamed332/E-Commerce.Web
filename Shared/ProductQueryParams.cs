﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
	public class ProductQueryParams
	{
		private const int DefaultPageSize = 5;
		private const int MaximumPageSize = 10;

		private int pageSize = DefaultPageSize;

		public int? BrandId { get; set; }
		public int? TypeId { get; set; }
		public ProductSortingOptions SortingOptions { get; set; }
		public string? SearchValue { get; set; }

		public int PageIndex { get; set; } = 1; 

		public int PageSize
		{
			get => pageSize;
			set => pageSize = (value > MaximumPageSize) ? MaximumPageSize : value;
		}
	
}
}
