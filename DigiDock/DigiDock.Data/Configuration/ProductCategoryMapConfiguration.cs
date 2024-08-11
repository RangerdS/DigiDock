using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Configuration
{
    public class ProductCategoryMapConfiguration : IEntityTypeConfiguration<ProductCategoryMap>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryMap> builder)
        {
            builder.ToTable("ProductCategoryMaps", "dbo");

            builder.ConfigureBaseEntity();

            builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });

            builder.HasOne(pc => pc.Product)
                   .WithMany(p => p.ProductCategoryMaps)
                   .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(pc => pc.Category)
                   .WithMany(c => c.ProductCategoryMaps)
                   .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
