using AutoMapper;
using Domain.Models.Products;
using Shared.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
	public class ProductProvider:Profile
	{
		public ProductProvider()
		{
			CreateMap<Product, ProductDto>()
				.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
				.ForMember(Dist => Dist.PictureUrl, options => options.MapFrom<ProductResolver>());

			CreateMap<ProductBrand, BrandDto>();
				
			CreateMap<ProductType, TypeDto>();
		}
	}
}
