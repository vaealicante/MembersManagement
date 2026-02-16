using System.Collections.Generic;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;
using MembersManagement.Application.AppMemberModule.BusinessLogic;
using MembersManagement.Domain.DomMemberModule.Entities;

namespace MembersManagement.Application.AppMemberModule.Services
{
    public class MemberService(MemberManager manager) : IMemberService
    {
        private readonly MemberManager _manager = manager;

        public IEnumerable<Member> GetMembers() => _manager.GetMembers();

        public IEnumerable<Member> GetDashboardData() => _manager.GetAllMembersRaw();

        public Member? GetMember(int id) => _manager.GetMemberById(id);

        public void CreateMember(Member member) => _manager.CreateMember(member);

        public void UpdateMember(Member member) => _manager.UpdateMember(member);

        public void DeleteMember(int id) => _manager.DeleteMember(id);
    }

}
