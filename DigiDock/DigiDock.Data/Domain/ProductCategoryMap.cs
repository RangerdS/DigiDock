using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
