using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain;

namespace MembersManagement.Infrastructure
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
        }

        protected MemberDbContext()
        {
        }
    }
}
