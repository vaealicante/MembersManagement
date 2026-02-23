using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipInterface;
using MembersManagement.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Infrastructure.InfraMembershipModule.MembershipRepositoryImplementation
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly MemberDbContext _context;

        public MembershipRepository(MemberDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Membership> GetAll()
            => _context.Memberships.AsNoTracking().ToList();

        public Membership? GetById(int id)
            => _context.Memberships
                       .AsNoTracking()
                       .FirstOrDefault(m => m.MembershipId == id);

        public void Add(Membership membership)
        {
            _context.Memberships.Add(membership);
        }

        public void Update(Membership membership)
        {
            _context.Memberships.Update(membership);
        }

        public void Delete(int id)
        {
            var membership = _context.Memberships.Find(id);
            if (membership != null)
                _context.Memberships.Remove(membership);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}