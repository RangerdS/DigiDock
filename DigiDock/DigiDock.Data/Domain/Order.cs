using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Domain
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } // fill here : add configure to max 9 digit
        public decimal CartTotal { get; set; }
        public decimal CouponTotal { get; set; }
        public string CouponCode { get; set; }
        public decimal PointTotal { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
