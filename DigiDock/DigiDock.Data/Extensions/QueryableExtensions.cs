using DigiDock.Base.Entity;

namespace DigiDock.Data.Extensions
{
    public static class QueryableExtensions
    {
        // Extension for IsDeleted status in BaseEntity
        public static IQueryable<TEntity> IsNotDeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : BaseEntity
        {
            return query.Where(x => !x.IsDeleted);
        }

        // Extension for IsActive status in BaseEntity
        public static IQueryable<TEntity> Active<TEntity>(this IQueryable<TEntity> query) where TEntity : BaseEntity
        {
            return query.Where(x => x.IsActive);
        }
    }
}
