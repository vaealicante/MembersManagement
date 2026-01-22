using MembersManagement.Domain.Entities;

namespace MembersManagement.Application.Services
{
    public interface IMemberService
    {
        IEnumerable<Member> GetMembers();
        Member? GetMember(int id);
        void CreateMember(Member member);
        void DeleteMember(Member member);
        void UpdateMember(Member member);
    }
}
