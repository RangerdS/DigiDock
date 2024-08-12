using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Users", Schema = "dbo")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string DigitalWalletInfo { get; set; }
        public decimal WalletBalance { get; set; }

        public List<UserPassword> UserPasswords { get; set; }
        public List<UserLogin> UserLogins { get; set; }
        public List<OrderDetail> OrderDetails{ get; set; }
        public List<Order> Orders{ get; set; }
    }
}
