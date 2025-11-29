using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class SaleTypeEntityConfiguration : IEntityTypeConfiguration<SaleType>
    {
        public void Configure(EntityTypeBuilder<SaleType> builder)
        {
            #region Basic configuration
            builder.HasKey(st => st.Id);
            builder.ToTable("SaleTypes");
            #endregion

            #region Property configurations
            builder.Property(st => st.Name).IsRequired().HasMaxLength(255);
            builder.Property(st => st.Description).IsRequired().HasMaxLength(1000);
            #endregion
        }
    }
}
