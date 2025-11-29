using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class FavoriteEntityConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            #region Basic configuration
            builder.HasKey(f => f.Id);
            builder.ToTable("Favorites");
            #endregion

            #region Property configurations
            builder.Property(f => f.ClientId).IsRequired();
            builder.Property(f => f.PropertyId).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(f => f.Properties)
                   .WithMany()
                   .HasForeignKey(f => f.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
