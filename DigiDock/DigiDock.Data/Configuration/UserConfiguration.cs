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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");

            builder.ConfigureBaseEntity();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.DigitalWalletInfo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.WalletBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(u => u.Email)
                .IsUnique();


            builder.HasMany(u => u.UserPasswords)
                   .WithOne(up => up.User)
                   .HasForeignKey(up => up.UserId);

            builder.HasMany(u => u.UserLogins)
                   .WithOne(ul => ul.User)
                   .HasForeignKey(ul => ul.UserId);
        }
    }
}
