using DigiDock.Base.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Requests
{
    public class AuthorizationRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
