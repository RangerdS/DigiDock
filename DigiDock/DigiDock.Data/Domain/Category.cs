using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Domain
{
    [Table("Categories", Schema = "dbo")]
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<string> Tags { get; set; }

        public List<ProductCategoryMap> ProductCategoryMaps { get; set; }
    }
}
