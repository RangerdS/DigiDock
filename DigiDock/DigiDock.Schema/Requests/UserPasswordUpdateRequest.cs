using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class UserPasswordUpdateRequest : BaseRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
