using AutoMapper;
using AutoMapper.Execution;
using Domain.Models.Products;
using Microsoft.Extensions.Configuration;
using Shared.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
	public class ProductResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
	{
		public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
		{
			if(string.IsNullOrEmpty(source.PictureUrl)) 
				return string.Empty;	

			else
			{
				var Url = $"{configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";

				return Url;
			}
		}
	}
}
