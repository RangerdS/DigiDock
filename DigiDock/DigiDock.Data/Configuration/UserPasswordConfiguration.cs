using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DigiDock.Data.Configuration
{
    public class UserPasswordConfiguration : IEntityTypeConfiguration<UserPassword>
    {
        public void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.ToTable("UserPasswords", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(up => up.Password)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(up => up.CreatedAt)
                .IsRequired();
        }
    }
}
