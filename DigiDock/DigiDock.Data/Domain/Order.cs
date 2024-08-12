using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Users", Schema = "dbo")]
    public class Order : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public string OrderNumber { get; set; } 
        public decimal CartTotal { get; set; }
        public decimal CouponTotal { get; set; }
        public string CouponCode { get; set; }
        public decimal PointTotal { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
