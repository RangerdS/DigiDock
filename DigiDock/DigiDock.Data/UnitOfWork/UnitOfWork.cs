using DigiDock.Data.Context;
using DigiDock.Data.Domain;
using DigiDock.Data.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DigiDock.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DigiDockMsDBContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private bool disposed = false;
        public IGenericRepository<Product> ProductRepository { get; private set; }
        public IGenericRepository<User> UserRepository { get; private set; }
        public IGenericRepository<UserPassword> UserPasswordRepository { get; private set; }
        public IGenericRepository<UserLogin> UserLoginRepository { get; private set; }
        public IGenericRepository<Coupon> CouponRepository { get; private set; }
        public IGenericRepository<OrderDetail> OrderDetailRepository { get; private set; }
        public IGenericRepository<Order> OrderRepository { get; private set; }
        public IGenericRepository<Category> CategoryRepository { get; private set; }
        public IGenericRepository<ProductCategoryMap> ProductCategoryMapRepository { get; private set; }

        public UnitOfWork(DigiDockMsDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            ProductRepository = new GenericRepository<Product>(this.context, httpContextAccessor);
            UserRepository = new GenericRepository<User>(this.context, httpContextAccessor);
            UserPasswordRepository = new GenericRepository<UserPassword>(this.context, httpContextAccessor);
            UserLoginRepository = new GenericRepository<UserLogin>(this.context, httpContextAccessor);
            CouponRepository = new GenericRepository<Coupon>(this.context, httpContextAccessor);
            OrderDetailRepository = new GenericRepository<OrderDetail>(this.context, httpContextAccessor);
            OrderRepository = new GenericRepository<Order>(this.context, httpContextAccessor);
            CategoryRepository = new GenericRepository<Category>(this.context, httpContextAccessor);
            ProductCategoryMapRepository = new GenericRepository<ProductCategoryMap>(this.context, httpContextAccessor);
        }


        public async Task CompleteAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task CompleteWithTransactionAsync()
        {
            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var dbTransaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await context.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await dbTransaction.RollbackAsync();
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
