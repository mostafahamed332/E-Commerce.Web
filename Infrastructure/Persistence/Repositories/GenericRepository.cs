using Docker.DotNet.Models;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class GenericRepository<TEntity, TKey>(StoredDBContext context) : IGenericRepository<TEntity, TKey> where TEntity : ModelBase<TKey>
	{
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		=> await context.Set<TEntity>().ToListAsync();

		public async Task<TEntity> GetByIdAsync(TKey id)
		=> await context.Set<TEntity>().FindAsync(id);

		public void Add(TEntity entity)
		=> context.Set<TEntity>().Add(entity);

		public void Delete(TEntity entity)
		=> context.Set<TEntity>().Remove(entity);

		public void Update(TEntity entity) 
		=> context.Set<TEntity>().Update(entity);

		public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec)
		{
			return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), Spec).ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync(ISpecification<TEntity, TKey> Spec)
		{
			return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), Spec).FirstOrDefaultAsync();
		}

		public async Task<int> CountAsync(ISpecification<TEntity, TKey> spec)
		{
			return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), spec).CountAsync();
		}
	}
}
