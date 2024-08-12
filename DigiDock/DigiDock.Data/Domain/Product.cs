using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Products", Schema = "dbo")]
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }

        public List<ProductCategoryMap> ProductCategoryMaps { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
