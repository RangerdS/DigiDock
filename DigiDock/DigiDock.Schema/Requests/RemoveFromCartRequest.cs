using DigiDock.Base.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Requests
{
    public class RemoveFromCartRequest : BaseRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
