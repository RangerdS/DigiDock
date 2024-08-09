using DigiDock.Data.Context;
using DigiDock.Data.Domain;
using DigiDock.Data.GenericRepository;
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
        private bool disposed = false;
        public IGenericRepository<Product> ProductRepository { get; private set; }
        public IGenericRepository<User> UserRepository { get; private set; }
        public IGenericRepository<UserLogin> UserLoginRepository { get; private set; }

        public UnitOfWork(DigiDockMsDBContext context)
        {
            this.context = context;

            ProductRepository = new GenericRepository<Product>(this.context);
            UserRepository = new GenericRepository<User>(this.context);
            UserLoginRepository = new GenericRepository<UserLogin>(this.context);
        }


        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task CompleteWithTransactionAsync()
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
