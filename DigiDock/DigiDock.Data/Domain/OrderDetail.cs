using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Users", Schema = "dbo")]
    public class OrderDetail : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long? OrderId { get; set; }
        public Order? Order { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public decimal? UnitPrice { get; set; }
        public long Quantity { get; set; }
    }
}
