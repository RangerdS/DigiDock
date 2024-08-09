using DigiDock.Base.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Schema.Responses
{
    public class ProductResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }
    }
}
