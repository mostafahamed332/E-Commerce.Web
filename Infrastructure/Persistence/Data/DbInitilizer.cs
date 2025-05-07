using Domain.Contracts;
using Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Data
{
	public class DbInitilizer(StoredDBContext context) : IDbIntializer
	{
		public async Task IntializeAsync()
		{


			if ((await context.Database.GetPendingMigrationsAsync()).Any())
			{
				await context.Database.MigrateAsync();
			}

			try
			{
				if (!context.Set<ProductBrand>().Any())
				{
					var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeds\brands.json");
					var Objects = JsonSerializer.Deserialize<List<ProductBrand>>(data);

					if (Objects is not null && Objects.Any())
					{
						context.Set<ProductBrand>().AddRange(Objects);
						await context.SaveChangesAsync();
					}
				}

				if (!context.Set<ProductType>().Any())
				{
					var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeds\types.json");
					var Objects = JsonSerializer.Deserialize<List<ProductType>>(data);

					if (Objects is not null && Objects.Any())
					{
						context.Set<ProductType>().AddRange(Objects);
						await context.SaveChangesAsync();
					}
				}

				if (!context.Set<Product>().Any())
				{
					var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeds\products.json");
					var Objects = JsonSerializer.Deserialize<List<Product>>(data);

					if (Objects is not null && Objects.Any())
					{
						context.Set<Product>().AddRange(Objects);
						await context.SaveChangesAsync();
					}
				}
			}
			catch (Exception ex)
			{

				throw;
			}

			
		}
	}
} 
