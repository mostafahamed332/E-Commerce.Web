using Abstraction;
using AutoMapper;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ServicesManager(IUnitOfWork unitOfWork, IMapper mapper) : IServicesManager
	{
		private readonly Lazy<IProductServices> _LazyProductServices = new Lazy<IProductServices>(() => new ProductServices(unitOfWork, mapper));

		public IProductServices ProductServices => _LazyProductServices.Value;
	}
}
