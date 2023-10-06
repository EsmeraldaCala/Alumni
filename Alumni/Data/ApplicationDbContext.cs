using Alumni.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alumni.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Alumni.Models.Alumni> Alumni { get; set; }
        public DbSet<Alumni.Models.FacultyRepresentative> FacultyRepresentatives { get; set; }

        public DbSet<Alumni.Models.Events> Events { get; set; }
        public DbSet<Alumni.Models.Admin> Admins { get; set; }
        public DbSet<Alumni.Models.JobOpportunity> JobOpportunities{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Alumni)
                .WithOne(b => b.ApplicationUser)
                .HasForeignKey<Models.Alumni>(b => b.UserId);


            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.FacultyRepresentative)
                .WithOne(b => b.ApplicationUser)
                .HasForeignKey<Models.FacultyRepresentative>(b => b.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Admin)
                .WithOne(b => b.ApplicationUser)
                .HasForeignKey<Models.Admin>(b => b.UserId);

            modelBuilder.Entity<Events>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(2000);

                entity.Property(e => e.EventLocation)
                    .HasMaxLength(500);

                entity.HasOne(e => e.ApplicationUser)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<JobOpportunity>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Details)
                    .HasMaxLength(2000);

                entity.Property(e => e.Summary)
                    .HasMaxLength(500);

                entity.HasOne(e => e.ApplicationUser)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Alumni", NormalizedName = "ALUMNI" },
                new IdentityRole<int> { Id = 2, Name = "Faculty Representative", NormalizedName = "FACULTY REPRESENTATIVE" },
                 new IdentityRole<int> { Id = 3, Name = "admin", NormalizedName = "ADMIN" }
            );

        }
    }
}