using Abstraction;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Products;
using Services.Specifications;
using Shared;
using Shared.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ProductServices(IUnitOfWork unitOfWork, IMapper mapper) : IProductServices
	{
		public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
		{
			var _Repository = unitOfWork.GetRepository<ProductBrand, int>();

			var Brands = await _Repository.GetAllAsync();

			var MappedBrands = mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);

			return MappedBrands;
		}

		public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(Shared.ProductQueryParams productQuery)
		{
			var _Repository = unitOfWork.GetRepository<Product, int>();
			var Spec = new ProductsWithBrandsAndTypeSpecification(productQuery); 
			var Products = await _Repository.GetAllAsync(Spec);

			var MappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
			var CountedProducts = Products.Count();
			
			var CountSpec = new ProductsWithBrandsAndTypeSpecification(productQuery);

			var TotalCount = await _Repository.CountAsync(CountSpec);
			return new PaginatedResult<ProductDto>(
				productQuery.PageIndex,
				CountedProducts,
				 TotalCount, 
				MappedProducts
			);
		}

		public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
		{
			var _Repository = unitOfWork.GetRepository<ProductType, int>();  


			var Types = await _Repository.GetAllAsync();

			var MappedTypes = mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);

			return MappedTypes;
		}

		public async Task<ProductDto> GetProductByIdAsync(int id)
		{
			var Spec = new ProductsWithBrandsAndTypeSpecification(id);

			var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);

			if (Product is null)
				throw new ProductNotFoundException(id);

			return mapper.Map<Product ,ProductDto>(Product);


		}
	}
}
