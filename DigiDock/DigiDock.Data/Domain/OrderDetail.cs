using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public long Quantity { get; set; }
    }
}
