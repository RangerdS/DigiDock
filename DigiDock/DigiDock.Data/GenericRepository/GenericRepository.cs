using DigiDock.Base.Entity;
using DigiDock.Data.Context;
using DigiDock.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

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
        // fill here add:  Where 
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

            var currentDate = DateTime.UtcNow;

            entity.CreateUserId = 1; // fill here change this
            entity.UpdateUserId = 1;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedAt = currentDate;
            entity.UpdatedAt = currentDate;

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
        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            // fill here
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(long Id)
        {
            // fill here
            throw new NotImplementedException();
        }
    }
}
