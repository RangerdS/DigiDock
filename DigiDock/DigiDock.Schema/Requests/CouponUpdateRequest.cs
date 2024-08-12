using DigiDock.Base.Requests;

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
