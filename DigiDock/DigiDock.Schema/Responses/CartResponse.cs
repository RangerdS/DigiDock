using DigiDock.Base.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Responses
{
    public class CartResponse : BaseResponse
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
        
    }
}
