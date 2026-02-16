using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain.DomMemberModule.Entities;

namespace MembersManagement.Infrastructure.InfraMemberModule.AppDbContext

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

    }
}
