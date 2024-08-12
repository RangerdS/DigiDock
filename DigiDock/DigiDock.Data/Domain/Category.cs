using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("Categories", Schema = "dbo")]
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<ProductCategoryMap> ProductCategoryMaps { get; set; }
    }
}
