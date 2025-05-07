using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
	public abstract class BaseSpecifications<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : ModelBase<TKey>
	{
		#region Criteria
		public BaseSpecifications(Expression<Func<TEntity, bool>>? PassedExpression)
		{
			Criteria = PassedExpression;
		}

		public Expression<Func<TEntity, bool>>? Criteria { get; private set; } 
		#endregion

		#region Includes
		public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = new List<Expression<Func<TEntity, object>>>();
		

		protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
		{
			IncludeExpressions.Add(includeExpression);
		}
		#endregion


		#region Sorting
		public  Expression<Func<TEntity, object>> OrderBy { get; private set; }
		public  Expression<Func<TEntity, object>> OrderByDesc { get; private set; }
		

		protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
		{
			OrderBy = orderByExpression;
		}

		protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
		{
			OrderByDesc = orderByDescExpression;
		}
		#endregion

		#region Pagination
		public  int Take { get; private set; }
		public  int Skip { get; private set; }
		public  bool IsPaginated { get; set; }

		protected void ApplyPagination(int PageSize, int PageIndex)
		{
			IsPaginated = true;
			Take = PageSize;
			Skip = PageSize * (PageIndex - 1);
		}
		#endregion
	}
}
