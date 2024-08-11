using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Domain
{
    public class Cart : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
