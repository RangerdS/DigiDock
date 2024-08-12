using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class UserRoleUpdateRequest : BaseRequest
    {
        public long UserId { get; set; }
        public string Role { get; set; }
    }
}
