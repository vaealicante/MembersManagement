using MembersManagement.Domain.DomMemberModule.Entities;
using System.Collections.Generic;

namespace MembersManagement.Application.AppMemberModule.ApplicationInterface
{
    public interface IMemberService
    {
        IEnumerable<Member> GetMembers();
        IEnumerable<Member> GetDashboardData();
        Member? GetMember(int id);
        void CreateMember(Member member);   
        void UpdateMember(Member member);
        void DeleteMember(int id);
    }
}
