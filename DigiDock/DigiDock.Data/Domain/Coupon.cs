using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Domain
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRedeemed { get; set; }
    }
}
