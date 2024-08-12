using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiDock.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(o => o.CartTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.CouponTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.OrderNumber)
                .HasMaxLength(9);

            builder.Property(o => o.CouponCode)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(o => o.PointTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(od => od.UserId);
        }
    }
}