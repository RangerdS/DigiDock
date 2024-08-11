using DigiDock.Base.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Responses
{
    public class CouponResponse : BaseResponse
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRedeemed { get; set; }
    }
}
