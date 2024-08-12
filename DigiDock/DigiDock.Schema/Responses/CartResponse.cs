using DigiDock.Base.Responses;

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
