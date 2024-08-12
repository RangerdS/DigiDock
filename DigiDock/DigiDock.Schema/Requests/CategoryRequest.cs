using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class CategoryRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
