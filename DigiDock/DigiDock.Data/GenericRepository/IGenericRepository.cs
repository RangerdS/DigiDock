using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task SaveAsync();
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(long Id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(long Id);
    }
}
