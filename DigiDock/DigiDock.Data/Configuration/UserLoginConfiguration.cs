using DigiDock.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DigiDock.Data.Configuration
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(ul => ul.IpAddress)
                   .IsRequired()
                   .HasMaxLength(40);

            builder.Property(ul => ul.IsLoginSuccess)
                   .IsRequired();

            builder.Property(ul => ul.ErrorMessage)
                   .HasMaxLength(500);
        }
    }
}
