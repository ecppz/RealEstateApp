using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ImprovementEntityConfiguration : IEntityTypeConfiguration<Improvement>
    {
        public void Configure(EntityTypeBuilder<Improvement> builder)
        {
            #region Basic configuration
            builder.HasKey(i => i.Id);
            builder.ToTable("Improvements");
            #endregion

            #region Property configurations
            builder.Property(i => i.Name).IsRequired().HasMaxLength(255);
            #endregion
        }
    }
}
