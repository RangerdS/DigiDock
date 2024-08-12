using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiDock.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(x => x.UnitPrice)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(od => od.Quantity)
                .IsRequired();

            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId);

            builder.HasOne(od => od.Product)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(od => od.ProductId);

            builder.HasOne(od => od.User)
                   .WithMany(u => u.OrderDetails)
                   .HasForeignKey(od => od.UserId);
        }
    }
}