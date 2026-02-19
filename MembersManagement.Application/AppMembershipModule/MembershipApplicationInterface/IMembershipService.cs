using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using System.Collections.Generic;

namespace MembersManagement.Application.AppMembershipModule.MembershipApplicationInterface
{
    public interface IMembershipService
    {
        IEnumerable<Membership> GetAllMemberships();
        Membership? GetMembership(int id);
        void CreateMembership(Membership membership);
        void UpdateMembership(Membership membership);
        void DeleteMembership(int id);
    }
}