using DigiDock.Base.Responses;

namespace DigiDock.Schema.Responses
{
    public class ProductWithCategoryResponse : BaseResponse
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
