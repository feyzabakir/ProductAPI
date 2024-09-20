using ProductAPI.Core.Models;
using ProductAPI.Core.Repositories;

namespace ProductAPI.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
