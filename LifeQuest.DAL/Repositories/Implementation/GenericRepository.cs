using LifeQuest.DAL.Data;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace LifeQuest.DAL.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        public readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet =  _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
          await  _dbSet.AddAsync(entity);
        }

        public async void Delete(int id)
        {
           T obj = await GetByIdAsync(id);

            if (obj != null)
            {
                if (obj is BaseEntity baseEntity)
                {
                    baseEntity.IsDeleted = true;
                    baseEntity.UpdateAt = DateTime.Now;
                    _dbSet.Update(obj);
                }
                _dbSet.Remove(obj);
            }
        }
        

        public async Task<IEnumerable<T>> GetAllAsync(T entity)
        {
           return await  _dbSet.ToListAsync();
        }

        public Task<IPagedList<T>> GetAllWithIncludeAsync(int pageNumber, int pageSize, Func<T, bool> predicate, params string[] Includes)
        {
            IQueryable<T> query = _dbSet;

            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }

            IPagedList<T> paged = query.Where(predicate).ToPagedList(pageNumber, pageSize);
            return Task.FromResult(paged);
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(Expression<Func<T, bool>> predicate, params string[] Includes)
        {
            IQueryable<T> query = _dbSet;
            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }

           return await query.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task<T> GetByIdWithIncludeAsync(Expression<Func<T, bool>> predicate, params string[] Includes)
        {
            IQueryable<T> query = _dbSet;

            if (Includes != null)
            {
                query = query.Where(predicate);
            }
            return query.FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            if(entity is BaseEntity baseEntity)
            {
                baseEntity.UpdateAt = DateTime.Now;
            }
            _dbSet.Update(entity);
        }
    }
}
