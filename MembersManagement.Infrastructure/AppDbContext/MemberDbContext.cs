using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;

namespace MembersManagement.Infrastructure.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

                // Ensure FK column is optional (matches int?)
                entity.Property(m => m.BranchId)
                      .IsRequired(false);

                entity.HasOne(m => m.Branch)
                      .WithMany()               // no navigation collection on Branch
                      .HasForeignKey(m => m.BranchId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅ safer than SetNull

                entity.HasOne(m => m.Membership)
                      .WithMany() // Or .WithMany(ms => ms.Members) if you add a collection to Membership
                      .HasForeignKey(m => m.MembershipId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevents deleting a membership that has active members
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(s => s.MembershipId);

                entity.Property(s => s.MembershipName)
                .IsRequired();
            });
        }
    }
}
