using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipInterface;
using MembersManagement.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace MembersManagement.Infrastructure.InfraMembershipModule.MembershipRepository
{
    public class MembershipRepository : IMembershipRepository

    {
        public void Add(Membership membership)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Membership> GetAll()
        {
            throw new NotImplementedException();
        }

        public Membership? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Membership membership)
        {
            throw new NotImplementedException();
        }
    }
}