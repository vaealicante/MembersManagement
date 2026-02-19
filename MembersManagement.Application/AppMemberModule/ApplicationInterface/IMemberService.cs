using MembersManagement.Application.AppMemberModule.BusinessLogic;
using MembersManagement.Domain.DomMemberModule.Entities;
using System.Collections.Generic;

namespace MembersManagement.Application.AppMemberModule.ApplicationInterface
{
    public interface IMemberService
    {
        // Active members only (used by Index page)
        IEnumerable<Member> GetMembers();

        // Dashboard data (can include inactive if needed)
        IEnumerable<Member> GetDashboardData();

        // Single member
        Member? GetMember(int id);

        void CreateMember(Member member);
        void UpdateMember(Member member);

        // Soft delete
        void DeleteMember(int id);
    }
}