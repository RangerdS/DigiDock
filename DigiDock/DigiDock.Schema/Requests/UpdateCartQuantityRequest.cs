using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class UpdateCartQuantityRequest : BaseRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
