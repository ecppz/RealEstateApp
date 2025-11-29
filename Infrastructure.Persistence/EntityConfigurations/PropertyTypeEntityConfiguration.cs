using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class PropertyTypeEntityConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            #region Basic configuration
            builder.HasKey(pt => pt.Id);
            builder.ToTable("PropertyTypes");
            #endregion

            #region Property configurations
            builder.Property(pt => pt.Name).IsRequired().HasMaxLength(255);
            builder.Property(pt => pt.Description).IsRequired().HasMaxLength(1000);
            #endregion
        }
    }
}
