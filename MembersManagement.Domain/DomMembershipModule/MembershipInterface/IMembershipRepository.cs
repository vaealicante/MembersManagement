using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using System.Collections.Generic;

namespace MembersManagement.Domain.DomMembershipModule.MembershipInterface
{
    public interface IMembershipRepository
    {
        IEnumerable<Membership> GetAll();
        Membership? GetById(int id);
        void Add(Membership membership);
        void Update(Membership membership);
        void Delete(int id);
        void SaveChanges();
    }
}
