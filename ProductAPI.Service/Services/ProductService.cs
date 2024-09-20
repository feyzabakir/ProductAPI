using ProductAPI.Core.Models;
using ProductAPI.Core.Repositories;
using ProductAPI.Core.Services;
using ProductAPI.Core.UnitOfWorks;

namespace ProductAPI.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
