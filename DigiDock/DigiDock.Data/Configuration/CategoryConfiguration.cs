using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiDock.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Url)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(c => c.ProductCategoryMaps)
                   .WithOne(pc => pc.Category)
                   .HasForeignKey(pc => pc.CategoryId);
        }
    }
}