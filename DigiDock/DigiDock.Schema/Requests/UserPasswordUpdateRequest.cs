using DigiDock.Base.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Requests
{
    public class UserPasswordUpdateRequest : BaseRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
