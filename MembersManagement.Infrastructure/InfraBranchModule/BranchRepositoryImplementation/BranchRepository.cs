using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomBranchModule.Interfaces;
using MembersManagement.Infrastructure.InfraBranchModule.BranchAppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Infrastructure.InfraBranchModule.BranchRepositoryImplementation
{
    public class BranchRepository : IBranchRepository
    {
        private readonly BranchDbContext _context;

        public BranchRepository(BranchDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Branch> GetAll()
    => _context.Branches.AsNoTracking().ToList();

        public Branch? GetById(int id)
            => _context.Branches.Find(id);

        public void Add(Branch branch)
            => _context.Branches.Add(branch);

        public void Update(Branch branch)
            => _context.Branches.Update(branch);

        public void Delete(int id)
        {
            var branch = _context.Branches.Find(id);
            if (branch != null)
                _context.Branches.Remove(branch);
        }

        public void SaveChanges()
            => _context.SaveChanges();
    }
}
