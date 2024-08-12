using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class CouponCreateRequest : BaseRequest
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
