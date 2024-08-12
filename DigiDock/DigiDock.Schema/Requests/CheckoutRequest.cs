using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class CheckoutRequest : BaseRequest
    {
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

        public string? CouponCode { get; set; }
    }
}
