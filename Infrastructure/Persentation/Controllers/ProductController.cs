using Abstraction;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers  
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController(IServicesManager servicesManager):ControllerBase
	{
		//GetAllProducts
		[HttpGet]
		//Get BaseUrl/api/Products
		public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts( [FromQuery] ProductQueryParams productQuery)
		{
			var Products = await servicesManager.ProductServices.GetAllProductsAsync(productQuery);

			return Ok(Products);
		}

		//GetAllBrands
		[HttpGet("brands")]
		//GET BaseUrl/api/brands
		public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
		{
			var Brands = await servicesManager.ProductServices.GetAllBrandsAsync();

			return Ok(Brands);
		}

		//GetAllTypes
		[HttpGet("Types")]
		//GET BaseUrl/api/Types
		public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
		{
			var Types = await servicesManager.ProductServices.GetAllTypesAsync();

			return Ok(Types);
		}


		[HttpGet("{id}")]
		//GET BaseUrl/api/Products/{id}
		public async Task<ActionResult<ProductDto>> GetProductById(int id)
		{
			var Product = await servicesManager.ProductServices.GetProductByIdAsync(id);
			if (Product is null) return NotFound();
			return Ok(Product);
		}
	}
}
