using System.Linq.Expressions;

namespace ProductAPI.Core.Services
{
    public interface IService<T> where T : class
    {
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}

