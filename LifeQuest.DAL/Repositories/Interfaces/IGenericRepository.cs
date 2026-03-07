using System.Linq.Expressions;
using X.PagedList;

namespace LifeQuest.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(T entity);
        public Task<T> GetByIdAsync(int id);
        public Task AddAsync (T entity);
        public void Update (T entity);
        public  void Delete (int id);
        public Task<T> GetByIdWithIncludeAsync(Expression<Func<T, bool>> predicate , params string[] Includes);
        public Task<IEnumerable<T>> GetAllWithIncludesAsync(Expression<Func<T, bool>> predicate, params string[] Includes);
        public Task<IPagedList<T>> GetAllWithIncludeAsync(int pageNumber , int PageSize ,Func<T, bool> predicate, params string[] Includes);
    }
}
