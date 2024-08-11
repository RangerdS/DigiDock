using DigiDock.Base.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Requests
{
    public class CouponUpdateRequest : BaseRequest
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsRedeemed { get; set; }
    }
}
