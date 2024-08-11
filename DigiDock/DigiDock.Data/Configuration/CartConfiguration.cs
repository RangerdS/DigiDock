using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiDock.Data.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(c => c.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(c => c.OrderDetails)
                   .WithOne()
                   .HasForeignKey(od => od.OrderId);
        }
    }
}