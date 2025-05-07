using Domain.Models.Products;
using Shared;
using System;

namespace Services.Specifications
{
	public class ProductsWithBrandsAndTypeSpecification : BaseSpecifications<Product, int>
	{
		public ProductsWithBrandsAndTypeSpecification(ProductQueryParams productQuery)
			: base(p =>
				(!productQuery.BrandId.HasValue || p.BrandId == productQuery.BrandId) &&
				(!productQuery.TypeId.HasValue || p.TypeId == productQuery.TypeId) &&
				(string.IsNullOrEmpty(productQuery.SearchValue) ||
				 p.Name.ToLower().Contains(productQuery.SearchValue.ToLower()))
			)
		{
			AddInclude(p => p.Brand);
			AddInclude(p => p.Type);

			switch (productQuery.SortingOptions)
			{
				case ProductSortingOptions.NameAsc:
					AddOrderBy(p => p.Name);
					break;
				case ProductSortingOptions.NameDesc:
					AddOrderByDesc(p => p.Name);
					break;
				case ProductSortingOptions.PriceAsc:
					AddOrderBy(p => p.Price);
					break;
				case ProductSortingOptions.PriceDesc:
					AddOrderByDesc(p => p.Price);
					break;
				default:
					break;
			}

			ApplyPagination(productQuery.PageSize, productQuery.PageIndex);
		}

		public ProductsWithBrandsAndTypeSpecification(int id)
			: base(p => p.Id == id)
		{
			AddInclude(p => p.Brand);
			AddInclude(p => p.Type);
		}
	}
}
