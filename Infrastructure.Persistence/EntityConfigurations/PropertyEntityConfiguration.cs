using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            #region Basic configuration
            builder.HasKey(p => p.Id);
            builder.ToTable("Properties");
            #endregion

            #region Property configurations
            builder.Property(p => p.Code).IsRequired().HasMaxLength(6);
            builder.Property(p => p.AgentId).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);

            #endregion

            #region Relationships
            builder.HasOne(p => p.PropertyType)
                   .WithMany(pt => pt.Properties)
                   .HasForeignKey(p => p.PropertyTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SaleType)
                   .WithMany(st => st.Properties)
                   .HasForeignKey(p => p.SaleTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Images)
                   .WithOne(pi => pi.Property)
                   .HasForeignKey(pi => pi.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(p => p.Improvements)
               .WithOne(pi => pi.Property)
               .HasForeignKey(pi => pi.PropertyId)
               .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
