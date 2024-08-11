﻿using DigiDock.Base.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Requests
{
    public class UserRoleUpdateRequest : BaseRequest
    {
        public long UserId { get; set; }
        public string Role { get; set; }
    }
}
