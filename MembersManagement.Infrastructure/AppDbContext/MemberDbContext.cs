using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMemberModule.Entities;

namespace MembersManagement.Infrastructure.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== BRANCH CONFIGURATION =====
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(b => b.BranchId);

                entity.Property(b => b.BranchName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.Location)
                      .HasMaxLength(150);
            });

            // ===== MEMBER CONFIGURATION & RELATIONSHIP =====
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.MemberID);

                // This establishes the physical Foreign Key in the database
                entity.HasOne(m => m.Branch)
                      .WithMany()
                      .HasForeignKey(m => m.BranchId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}