using DigiDock.Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Data.Configuration
{
    public static class EntityConfigurationExtension
    {
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : BaseEntity
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.DeletedAt);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CreateUserId)
                .IsRequired();

            builder.Property(x => x.UpdateUserId)
                .IsRequired();

            builder.Property(x => x.DeleteUserId);
        }
    }
}
