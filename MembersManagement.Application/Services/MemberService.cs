using MembersManagement.Application.BusinessLogic;
using MembersManagement.Domain.Entities;
using MembersManagement.Application.ApplicationInterface;
using System.Collections.Generic;

namespace MembersManagement.Application.Services
{
    public class MemberService(MemberManager manager) : IMemberService
    {
        private readonly MemberManager _manager = manager;

        public IEnumerable<Member> GetMembers() => _manager.GetMembers();

        public IEnumerable<Member> GetDashboardData() => _manager.GetAllMembersRaw();

        public Member? GetMember(int id) => _manager.GetMember(id);

        public void CreateMember(Member member) => _manager.CreateMember(member);

        public void UpdateMember(Member member) => _manager.UpdateMember(member);

        public void DeleteMember(int id) => _manager.DeleteMember(id);
    }

}
