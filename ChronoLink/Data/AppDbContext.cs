using ChronoLink.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        // DbSet properties for each entity
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<WorkspaceUser> WorkspaceUsers { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure many-to-many relationship for WorkspaceUser
            modelBuilder.Entity<WorkspaceUser>().HasKey(wu => new { wu.UserId, wu.WorkspaceId });

            modelBuilder
                .Entity<WorkspaceUser>()
                .HasOne(wu => wu.User)
                .WithMany(u => u.WorkspaceUsers)
                .HasForeignKey(wu => wu.UserId);

            modelBuilder
                .Entity<WorkspaceUser>()
                .HasOne(wu => wu.Workspace)
                .WithMany(w => w.WorkspaceUsers)
                .HasForeignKey(wu => wu.WorkspaceId);

            // Configure one-to-many relationship between User and Calendar (personal calendars)
            modelBuilder
                .Entity<Calendar>()
                .HasOne(c => c.User)
                .WithMany(u => u.PersonalCalendars)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure one-to-one relationship between Workspace and Calendar (shared calendar)
            modelBuilder
                .Entity<Workspace>()
                .HasOne(w => w.SharedCalendar)
                .WithOne(c => c.Workspace)
                .HasForeignKey<Calendar>(c => c.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade); // Delete shared calendar if workspace is deleted

            // Configure one-to-many relationship between Calendar and Event
            modelBuilder
                .Entity<Event>()
                .HasOne(e => e.Calendar)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CalendarId)
                .OnDelete(DeleteBehavior.Cascade); // Delete events if calendar is deleted

            // Configure one-to-many relationship between User and QuestionResponse
            modelBuilder
                .Entity<QuestionResponse>()
                .HasOne(qr => qr.User)
                .WithMany(u => u.QuestionResponses)
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete question/responses if user is deleted
        }
    }
}
