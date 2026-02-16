using MembersManagement.Domain.DomMemberModule.Entities;
using System.Collections.Generic;

namespace MembersManagement.Domain.DomMemberModule.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        void Add(Member member);
        void Update(Member member);
        void Delete(int id);
        void SaveChanges();
    }
}
