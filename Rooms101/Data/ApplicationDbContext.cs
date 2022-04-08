using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rooms101.Areas.Identity.Data;
using Rooms101.Models;

namespace Rooms101.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Meeting>()
                .Property(m => m.CreateMoment)
                .HasDefaultValueSql("getdate()");

            builder.Entity<Meeting>()
                .Property(m => m.Cancelled)
                .HasDefaultValueSql("(0)");

            base.OnModelCreating(builder);
        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<MeetingAttendee> MeetingAttendees { get; set; }
    }
}