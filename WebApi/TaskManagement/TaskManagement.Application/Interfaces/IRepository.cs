
using System.Linq.Expressions;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync();

        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            IQueryable<TEntity> query,
            int pageIndex,
            int pageSize);
    }
}