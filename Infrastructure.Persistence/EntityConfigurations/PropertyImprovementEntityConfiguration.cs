using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class PropertyImprovementEntityConfiguration : IEntityTypeConfiguration<PropertyImprovement>
    {
        public void Configure(EntityTypeBuilder<PropertyImprovement> builder)
        {
            builder.HasKey(pi => pi.Id);

            #region Relationships
            builder.HasOne(pi => pi.Property)
                   .WithMany(p => p.Improvements)
                   .HasForeignKey(pi => pi.PropertyId);

            builder.HasOne(pi => pi.Improvement)
                   .WithMany(i => i.PropertyImprovements)
                   .HasForeignKey(pi => pi.ImprovementId);
            #endregion
        }
    }

}
