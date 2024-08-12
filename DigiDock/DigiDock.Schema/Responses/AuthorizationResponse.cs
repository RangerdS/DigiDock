using DigiDock.Base.Responses;

namespace DigiDock.Schema.Responses
{
    public class AuthorizationResponse : BaseResponse
    {
        public DateTime ExpireTime { get; set; }
        public string AccessToken { get; set; }
        public string Email { get; set; }
    }
}
