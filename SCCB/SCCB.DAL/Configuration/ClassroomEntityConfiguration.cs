using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Configuration
{
    public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Number)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Building)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(x => x.Bookings)
               .WithOne(x => x.Classroom)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.Classroom)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
