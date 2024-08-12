using DigiDock.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiDock.Data.Domain
{
    [Table("ProductCategoryMaps", Schema = "dbo")]
    public class ProductCategoryMap : BaseEntity
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
