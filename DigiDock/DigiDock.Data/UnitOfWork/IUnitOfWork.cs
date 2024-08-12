using DigiDock.Data.Domain;
using DigiDock.Data.GenericRepository;

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
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<ProductCategoryMap> ProductCategoryMapRepository { get; }
    }
}
