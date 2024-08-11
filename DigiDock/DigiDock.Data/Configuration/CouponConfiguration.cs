using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiDock.Data.Configuration
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons", "dbo");

            builder.ConfigureBaseEntity();

            // Alanlar
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.ExpiryDate)
                .IsRequired();

            builder.Property(c => c.IsRedeemed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(c => c.Code).IsUnique();
        }
    }
}