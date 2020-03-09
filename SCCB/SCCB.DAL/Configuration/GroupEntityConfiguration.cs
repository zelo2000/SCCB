using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Configuration
{
    public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Group)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.Group)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
