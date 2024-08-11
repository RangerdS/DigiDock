using DigiDock.Data.Domain;
using DigiDock.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
        Task CompleteWithTransactionAsync();

        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<UserPassword> UserPasswordRepository { get; }
        IGenericRepository<UserLogin> UserLoginRepository { get; }
        IGenericRepository<Coupon> CouponRepository { get; }
    }
}
