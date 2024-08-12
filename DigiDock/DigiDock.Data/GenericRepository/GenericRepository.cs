using DigiDock.Base.Entity;
using DigiDock.Data.Context;
using DigiDock.Data.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigiDock.Data.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DigiDockMsDBContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private int currentUserId = 1;

        public GenericRepository(DigiDockMsDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim != null)
            {
                currentUserId = int.Parse(userIdClaim);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while saving changes.", ex);
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                return await context.Set<TEntity>()
                    .AsNoTracking()
                    .IsNotDeleted()
                    .Active()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities.", ex);
            }
        }

        public async Task<TEntity?> GetByIdAsync(long Id)
        {
            try
            {
                return await context.Set<TEntity>()
                    .AsNoTracking()
                    .Active()
                    .IsNotDeleted()
                    .SingleOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task InsertAsync(TEntity entity)
        {
            try
            {
                if (entity is null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
                }

                var currentDate = DateTime.UtcNow;

                entity.CreateUserId = currentUserId;
                entity.UpdateUserId = currentUserId;
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.CreatedAt = currentDate;
                entity.UpdatedAt = currentDate;

                await context.Set<TEntity>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the entity.", ex);
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                if (entity is null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
                }

                var trackedEntity = context.Set<TEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
                if (trackedEntity != null)
                {
                    context.Entry(trackedEntity).State = EntityState.Detached;
                }

                entity.UpdateUserId = currentUserId;
                entity.UpdatedAt = DateTime.UtcNow;

                context.Set<TEntity>().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the entity.", ex);
            }
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            try
            {
                IQueryable<TEntity> query = context.Set<TEntity>()
                    .AsNoTracking()
                    .Active()
                    .IsNotDeleted();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<TEntity> LastOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            try
            {
                IQueryable<TEntity> query = context.Set<TEntity>()
                    .AsNoTracking()
                    .Active()
                    .IsNotDeleted();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                query = query.OrderByDescending(e => e.Id);

                return await query.LastOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            try
            {
                var query = context.Set<TEntity>().Where(expression)
                    .AsNoTracking()
                    .Active()
                    .IsNotDeleted()
                    .Where(expression);

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities.", ex);
            }
        }

        public void Delete(TEntity entity)
        {
            try
            {
                if (entity is null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
                }

                var trackedEntity = context.Set<TEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
                if (trackedEntity != null)
                {
                    context.Entry(trackedEntity).State = EntityState.Detached;
                }

                entity.IsDeleted = true;
                entity.IsActive = false;
                entity.DeleteUserId = currentUserId;
                entity.DeletedAt = DateTime.UtcNow;

                context.Set<TEntity>().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the entity.", ex);
            }
        }

        public async Task Delete(long Id)
        {
            try
            {
                var entity = await GetByIdAsync(Id);
                if (entity is null)
                {
                    throw new KeyNotFoundException("Entity not found.");
                }

                Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the entity by Id.", ex);
            }
        }
    }
}
