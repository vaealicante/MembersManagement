using FluentValidation;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMemberModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Application.AppMemberModule.BusinessLogic
{
    // Using C# 12 Primary Constructor syntax
    public class MemberManager(
        IMemberRepository memberRepository,
        IValidator<Member> validator)
    {
        private readonly IMemberRepository _memberRepository = memberRepository;
        private readonly IValidator<Member> _validator = validator;

        /// <summary>
        /// Validates and saves a new member. IsActive defaults to true.
        /// </summary>
        public void CreateMember(Member member)
        {
            // Enforces the rules (Optional Birthdate/Email/Contact are handled here)
            _validator.ValidateAndThrow(member);

            member.IsActive = true;
            member.DateCreated = DateTime.UtcNow;

            _memberRepository.Add(member);
            _memberRepository.SaveChanges();
        }

        /// <summary>
        /// Returns only members where IsActive is true.
        /// </summary>
        public IEnumerable<Member> GetMembers()
        {
            return _memberRepository
                .GetAll()
                .Where(m => m.IsActive);
        }

        public Member? GetMemberById(int id)
        {
            return _memberRepository.GetById(id);
        }

        /// <summary>
        /// Validates changes and updates the member record.
        /// </summary>
        public void UpdateMember(Member member)
        {
            // Re-validates age range and format rules if data was changed
            _validator.ValidateAndThrow(member);

            _memberRepository.Update(member);
            _memberRepository.SaveChanges();
        }

        /// <summary>
        /// Performs a Soft Delete by switching the IsActive flag.
        /// </summary>
        public void DeleteMember(int id)
        {
            var member = _memberRepository.GetById(id);
            if (member != null)
            {
                member.IsActive = false; // The Soft Delete
                _memberRepository.Update(member);
                _memberRepository.SaveChanges();
            }
        }
        /// <summary>
        /// Administrative method to see all records regardless of status.
        /// </summary>
        public IEnumerable<Member> GetAllMembersRaw()
        {
            return _memberRepository.GetAll();
        }
    }
}