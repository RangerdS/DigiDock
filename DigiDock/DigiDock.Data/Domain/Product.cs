using DigiDock.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Domain
{
    [Table("Products", Schema = "dbo")]
    public class Product : BaseEntity
    {
        //Ürün (Kategori, adi, özellikleri, açıklama, aktiflik, puan kazandırma yüzdesi, max puan tutarı
        public string Name { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }
        //public List<ProductCategory> ProductCategories { get; set; } // fill here


    }
}
