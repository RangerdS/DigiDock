using DigiDock.Base.Entity;
using DigiDock.Data.Configuration;
using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigiDock.Data.Context
{
    public class DigiDockMsDBContext : DbContext
    {
        public DigiDockMsDBContext(DbContextOptions<DigiDockMsDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserPasswordConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
        }

        /*
        // Fill here
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreateUser = GetCurrentUserId(); // Replace with actual user context
                }

                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdateUser = GetCurrentUserId(); // Replace with actual user context

                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    entity.IsActive = false;
                    entity.DeletedAt = DateTime.UtcNow;
                    entity.DeleteUser = GetCurrentUserId(); // Replace with actual user context
                }
            }

            return base.SaveChanges();
        }*/
    }
}
