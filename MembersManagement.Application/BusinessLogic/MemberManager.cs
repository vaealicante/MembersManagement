using FluentValidation;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace MembersManagement.Application.BusinessLogic
{
    public class MemberManager
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<Member> _validator;

        public MemberManager(IMemberRepository memberRepository, IValidator<Member> validator)
        {
            _memberRepository = memberRepository;
            _validator = validator;
        }

        public void CreateMember(Member member)
        {
            _validator.ValidateAndThrow(member);
            member.IsActive = true;
            member.DateCreated = DateTime.UtcNow;
            _memberRepository.Add(member);
            _memberRepository.SaveChanges();
        }

        public void UpdateMember(Member member)
        {
            _validator.ValidateAndThrow(member);

            var existing = _memberRepository.GetById(member.MemberID)
                ?? throw new KeyNotFoundException("Member not found.");

            existing.FirstName = member.FirstName;
            existing.LastName = member.LastName;
            existing.BirthDate = member.BirthDate;
            existing.Address = member.Address;
            existing.Branch = member.Branch;
            existing.ContactNo = member.ContactNo;
            existing.Email = member.Email;
            existing.IsActive = member.IsActive;

            _memberRepository.Update(existing);
            _memberRepository.SaveChanges();
        }

        public void DeleteMember(int memberId)
        {
            var existing = _memberRepository.GetById(memberId)
                ?? throw new KeyNotFoundException("Member not found.");

            _memberRepository.Delete(memberId);
            _memberRepository.SaveChanges();
        }

        public Member? GetMember(int id) => _memberRepository.GetById(id);

        public IEnumerable<Member> GetMembers() => _memberRepository.GetAll();
    }
}
