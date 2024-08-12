using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class AuthorizationRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
