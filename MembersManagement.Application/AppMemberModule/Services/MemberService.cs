using System.Collections.Generic;

using MembersManagement.Application.AppMemberModule.ApplicationInterface;

using MembersManagement.Application.AppMemberModule.BusinessLogic;

using MembersManagement.Domain.DomMemberModule.Entities;


namespace MembersManagement.Application.AppMemberModule.Services

{
    public class MemberService : IMemberService

    {
        private readonly MemberManager _memberManager;

        public MemberService(MemberManager memberManager)
        {
            _memberManager = memberManager;
        }
        public IEnumerable<Member> GetMembers()
        {

            // Active members only
            return _memberManager.GetMembers();
        }
        public IEnumerable<Member> GetDashboardData()
        {
            // Dashboard can see ALL (active + inactive)
            return _memberManager.GetAllMembersRaw();
        }
        public Member? GetMember(int id)
        {
            return _memberManager.GetMemberById(id);
        }
        public void CreateMember(Member member)
        {
            _memberManager.CreateMember(member);
        }
        public void UpdateMember(Member member)
        {
            _memberManager.UpdateMember(member);
        }
        public void DeleteMember(int id)
        {
            // Soft delete handled inside manager
            _memberManager.DeleteMember(id);
        }
    }
}