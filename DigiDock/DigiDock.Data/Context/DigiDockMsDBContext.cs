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
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryMapConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
        }
    }
}
