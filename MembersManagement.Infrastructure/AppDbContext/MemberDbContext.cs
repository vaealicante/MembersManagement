using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain.Entities;

namespace MembersManagement.Infrastructure.AppDbContext

{
    public class MemberDbContext : DbContext
    {
        public MemberDbContext(DbContextOptions <MemberDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasKey(m => m.MemberID);

            // Global soft-delete filter
            modelBuilder.Entity<Member>()
                .HasQueryFilter(m => m.IsActive);
        }

    }
}
