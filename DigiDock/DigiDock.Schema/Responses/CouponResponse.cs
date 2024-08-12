using DigiDock.Base.Responses;

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
