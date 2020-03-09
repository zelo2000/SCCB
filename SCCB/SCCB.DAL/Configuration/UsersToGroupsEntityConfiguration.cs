using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Configuration
{
    public class UsersToGroupsEntityConfiguration : IEntityTypeConfiguration<UsersToGroups>
    {
        public void Configure(EntityTypeBuilder<UsersToGroups> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UsersToGroups)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Group)
                .WithMany(x => x.UsersToGroups)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
