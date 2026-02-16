using Microsoft.EntityFrameworkCore;
using MembersManagement.Domain.DomBranchModule.BranchEntities;

namespace MembersManagement.Infrastructure.InfraBranchModule.BranchAppDbContext

{
    public class BranchDbContext : DbContext
    {
        public BranchDbContext(DbContextOptions<BranchDbContext> options) : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .HasKey(m => m.BranchID);


        }

    }
}
