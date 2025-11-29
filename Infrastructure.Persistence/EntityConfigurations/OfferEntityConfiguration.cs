using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class OfferEntityConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            #region Basic configuration
            builder.HasKey(o => o.Id);
            builder.ToTable("Offers");
            #endregion

            #region Property configurations
            builder.Property(o => o.PropertyId).IsRequired();
            builder.Property(o => o.ClientId).IsRequired();
            builder.Property(o => o.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(o => o.Date).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(o => o.Property)
                   .WithMany()
                   .HasForeignKey(o => o.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
