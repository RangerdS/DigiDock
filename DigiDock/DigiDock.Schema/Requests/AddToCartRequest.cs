using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class AddToCartRequest : BaseRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
