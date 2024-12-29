using System.Text.RegularExpressions;
using Authentication_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using TMS_WebAPI.Models;

namespace Authentication_WebAPI.Context
{
    public class AppDBContext :DbContext
    {
        public AppDBContext()
        {
            
        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role() { RoleId = 1, RoleName = "Admin" },
                 new Role() { RoleId = 2, RoleName = "Manager" },
                 new Role() { RoleId = 3, RoleName = "User" }
                );

            //modelBuilder.Entity<Enrollment>()
            //        .HasRequired(m => m.User)
            //        .WithMany(t => t.HomeMatches)
            //        .HasForeignKey(m => m.HomeTeamId)
            //        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Match>()
            //            .HasRequired(m => m.GuestTeam)
            //            .WithMany(t => t.AwayMatches)
            //            .HasForeignKey(m => m.GuestTeamId)
            //            .WillCascadeOnDelete(false);
        }
    }
}
