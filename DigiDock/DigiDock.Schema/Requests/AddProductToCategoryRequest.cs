using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class AddProductToCategoryRequest : BaseRequest
    {
        public long CategoryId { get; set; }
        public long ProductId { get; set; }
    }
}
