using DigiDock.Base.Entity;
using DigiDock.Data.Context;
using DigiDock.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DigiDockMsDBContext context;
        public GenericRepository(DigiDockMsDBContext context)
        {
            this.context = context;
        }


        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>()
                .AsNoTracking()
                .IsNotDeleted()
                .Active()
                .ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(long Id)
        {
            return await context.Set<TEntity>()
                .AsNoTracking()
                .Active()
                .IsNotDeleted()
                .SingleOrDefaultAsync(x=> x.Id == Id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity is null)
            {
                // fill here : Capsule all codes with try catch
                throw new ArgumentNullException(nameof(entity));
            }

            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            // Fill Here : Add user info's to all command methods
            context.Set<TEntity>().Update(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
