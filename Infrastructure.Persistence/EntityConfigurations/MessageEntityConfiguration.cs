using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            #region Basic configuration
            builder.HasKey(m => m.Id);
            builder.ToTable("Messages");
            #endregion

            #region Property configurations
            builder.Property(m => m.PropertyId).IsRequired();
            builder.Property(m => m.SenderId).IsRequired();
            builder.Property(m => m.ReceiverId).IsRequired();
            builder.Property(m => m.Content).IsRequired().HasMaxLength(2000);
            builder.Property(m => m.SentAt).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(m => m.Property)
                   .WithMany()
                   .HasForeignKey(m => m.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
