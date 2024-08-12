using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class CategoryUpdateRequest : BaseRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
