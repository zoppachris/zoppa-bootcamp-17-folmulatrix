using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>(entity =>
             {
                 entity.HasKey(rt => rt.Id);
                 entity.Property(rt => rt.Id).ValueGeneratedOnAdd();
                 entity.HasIndex(rt => rt.Token).IsUnique();
                 entity.HasOne(rt => rt.User)
                     .WithMany()
                     .HasForeignKey(rt => rt.UserId)
                     .OnDelete(DeleteBehavior.Cascade);
             });

            builder.Entity<Project>(entity =>
           {
               entity.HasKey(p => p.Id);
               entity.Property(p => p.Id).ValueGeneratedOnAdd(); // Guid auto-generate
               entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
               entity.HasOne(p => p.Owner)
                   .WithMany()
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
           });

            builder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => pm.Id);
                entity.Property(pm => pm.Id).ValueGeneratedOnAdd();
                entity.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pm => pm.User)
                    .WithMany()
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(pm => new { pm.ProjectId, pm.UserId }).IsUnique();
            });

            builder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(t => t.AssignedUser)
                    .WithMany()
                    .HasForeignKey(t => t.AssignedUserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}