using MembersManagement.Domain.Entities;
using System.Collections.Generic;

namespace MembersManagement.Application.ApplicationInterface
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
