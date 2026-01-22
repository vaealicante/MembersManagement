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
            // Primary Key
            modelBuilder.Entity<Member>()
                .HasKey(m => m.MemberID);

        }

        protected MemberDbContext()
        {
        }
    }
}
