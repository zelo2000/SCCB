using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Configuration
{
    public class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Weekday)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(x => x.IsDenominator)
                .IsRequired();

            builder.Property(x => x.IsEnumerator)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(x => x.LessonNumber)
                .IsRequired();
        }
    }
}
