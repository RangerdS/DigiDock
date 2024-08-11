using DigiDock.Base.Token;
using DigiDock.Data.Context;
using DigiDock.Data.Domain;
using DigiDock.Data.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public UnitOfWork(DigiDockMsDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            ProductRepository = new GenericRepository<Product>(this.context, httpContextAccessor);
            UserRepository = new GenericRepository<User>(this.context, httpContextAccessor);
            UserPasswordRepository = new GenericRepository<UserPassword>(this.context, httpContextAccessor);
            UserLoginRepository = new GenericRepository<UserLogin>(this.context, httpContextAccessor);
            CouponRepository = new GenericRepository<Coupon>(this.context, httpContextAccessor);
        }


        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
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
