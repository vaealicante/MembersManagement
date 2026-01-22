using System.Collections.Generic;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;

namespace MembersManagement.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Member> GetMembers()
        {
            return _repository.GetAll();
        }

        public Member? GetMember(int id)
        {
            return _repository.GetById(id);
        }

        public void CreateMember(Member member)
        {
            _repository.Add(member);
            _repository.SaveChanges();
        }

        public void UpdateMember(Member member)
        {
            _repository.Update(member);
            _repository.SaveChanges();
        }

        public void DeleteMember(Member member)
        {
            _repository.Delete(member.MemberID);
            _repository.SaveChanges();
        }
    }
}
