using Domain.Models;
using Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IUnitOfWork
	{
		 //IGenericRepository<Product, int> ProductRepo { get; set; }
		 Task<int> SaveChangesAsync();

		IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : ModelBase<TKey>;
	}
}
