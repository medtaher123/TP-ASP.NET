using ChronoLink.Models;
using Microsoft.EntityFrameworkCore;
using Task = ChronoLink.Models.Task;

namespace ChronoLink.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkspaceUser> WorkspaceUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure WorkspaceUser as a many-to-many join table between User and Workspace
            modelBuilder
                .Entity<WorkspaceUser>()
                .HasOne(wu => wu.User)
                .WithMany(u => u.WorkspaceUsers)
                .HasForeignKey(wu => wu.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete WorkspaceUser if User is deleted

            modelBuilder
                .Entity<WorkspaceUser>()
                .HasOne(wu => wu.Workspace)
                .WithMany(w => w.WorkspaceUsers)
                .HasForeignKey(wu => wu.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade); // Delete WorkspaceUser if Workspace is deleted

            // Configure Task entity
            modelBuilder
                .Entity<Task>()
                .HasOne(t => t.WorkspaceUser)
                .WithMany(wu => wu.Tasks)
                .HasForeignKey(t => t.WorkspaceUserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Task if WorkspaceUser is deleted

            // Configure QuestionResponse entity
            modelBuilder
                .Entity<QuestionResponse>()
                .HasOne(qr => qr.User)
                .WithMany(u => u.QuestionResponses)
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete QuestionResponse if User is deleted

            // Configure User entity (IdentityUser)
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name).HasMaxLength(100).IsRequired();

                entity
                    .HasMany(u => u.WorkspaceUsers)
                    .WithOne(wu => wu.User)
                    .HasForeignKey(wu => wu.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasMany(u => u.QuestionResponses)
                    .WithOne(qr => qr.User)
                    .HasForeignKey(qr => qr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Workspace entity
            modelBuilder.Entity<Workspace>(entity =>
            {
                entity.Property(w => w.Name).HasMaxLength(100).IsRequired();

                entity
                    .HasMany(w => w.WorkspaceUsers)
                    .WithOne(wu => wu.Workspace)
                    .HasForeignKey(wu => wu.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
