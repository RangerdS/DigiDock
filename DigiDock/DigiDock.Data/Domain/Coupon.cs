using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Coupons", Schema = "dbo")]
    public class Coupon : BaseEntity
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRedeemed { get; set; }
    }
}
