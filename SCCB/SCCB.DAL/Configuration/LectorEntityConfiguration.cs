using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Configuration
{
    public class LectorEntityConfiguration : IEntityTypeConfiguration<Lector>
    {
        public void Configure(EntityTypeBuilder<Lector> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Position)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.Lector)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
