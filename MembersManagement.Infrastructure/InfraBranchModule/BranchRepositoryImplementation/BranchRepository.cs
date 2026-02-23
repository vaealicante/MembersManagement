using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomBranchModule.BranchInterfaces;
using MembersManagement.Infrastructure.AppDbContext; // AppDbContext namespace
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Infrastructure.InfraBranchModule.BranchRepositoryImplementation
{
    public class BranchRepository : IBranchRepository
    {
        private readonly MemberDbContext _context;

        public BranchRepository(MemberDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Branch> GetAll()
            => _context.Branches.AsNoTracking().ToList();

        public Branch? GetById(int id)
            => _context.Branches.Find(id);

        public void Add(Branch branch)
        {
            _context.Branches.Add(branch);
            _context.SaveChanges();
        }

        public void Update(Branch branch)
        {
            _context.Branches.Update(branch);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var branch = _context.Branches.Find(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
