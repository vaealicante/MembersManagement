using MembersManagement.Application.AppMembershipModule.MembershipApplicationInterface;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipInterface;

namespace MembersManagement.Application.AppMembershipModule.MembershipServices
{
    public class MembershipService : IMembershipService
    {
        public void CreateMembership(Membership membership)
        {
            throw new NotImplementedException();
        }

        public void DeleteMembership(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Membership> GetAllMemberships()
        {
            throw new NotImplementedException();
        }

        public Membership? GetMembership(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMembership(Membership membership)
        {
            throw new NotImplementedException();
        }
    }
}