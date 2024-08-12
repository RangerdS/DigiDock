using DigiDock.Base.Requests;

namespace DigiDock.Schema.Requests
{
    public class SignInRequest : BaseRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
