using DigiDock.Base.Entity;
using System.Linq.Expressions;

namespace DigiDock.Data.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task SaveAsync();
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(long Id);
        Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task<TEntity> LastOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Delete(long Id);
    }
}
