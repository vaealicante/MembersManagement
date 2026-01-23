using FluentValidation;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using System;

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

        // Business logic to create a member
        public void CreateMember(Member member)
        {
            // Validate
            _validator.ValidateAndThrow(member);

            // Business defaults
            member.IsActive = true;
            member.DateCreated = DateTime.UtcNow;

            // Save
            _memberRepository.Add(member);
            _memberRepository.SaveChanges();
        }

        // Update business logic
        public void UpdateMember(Member member)
        {
            _validator.ValidateAndThrow(member);

            var existing = _memberRepository.GetById(member.MemberID);
            if (existing == null)
                throw new KeyNotFoundException("Member not found.");

            // Update allowed fields
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

        // Delete business logic
        public void DeleteMember(int memberId)
        {
            var existing = _memberRepository.GetById(memberId);
            if (existing == null)
                throw new KeyNotFoundException("Member not found.");

            _memberRepository.Delete(memberId);
            _memberRepository.SaveChanges();
        }

        public Member? GetMember(int id) => _memberRepository.GetById(id);

        public IEnumerable<Member> GetMembers() => _memberRepository.GetAll();
    }
}
