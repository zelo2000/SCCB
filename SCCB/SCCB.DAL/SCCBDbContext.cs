using Microsoft.EntityFrameworkCore;
using SCCB.DAL.Configuration;
using SCCB.DAL.Entities;
using SCCB.DAL.Extensions;

namespace SCCB.DAL
{
    public class SCCBDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Lector> Lectors { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UsersToGroups> UsersToGroups { get; set; }

        public SCCBDbContext(DbContextOptions<SCCBDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LectorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdminEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClassroomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UsersToGroupsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LessonEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BookingEntityConfiguration());

            modelBuilder.Seed();
        }
    }
}