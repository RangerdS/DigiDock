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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Features)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.RewardPointsPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.MaxRewardPoints)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Many-to-Many relationship configuration
            // fill here
            /*builder
                .HasMany(x => x.ProductCategories)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);*/ // fill here
        }
    }
}
