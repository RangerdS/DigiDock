using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class ProductWithCategoryRequest : BaseRequest
    {
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
    }
}
