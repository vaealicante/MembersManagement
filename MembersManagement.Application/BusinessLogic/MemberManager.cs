using FluentValidation;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Application.BusinessLogic
{
    public class MemberManager
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<Member> _validator;

        public MemberManager(
            IMemberRepository memberRepository,
            IValidator<Member> validator)
        {
            _memberRepository = memberRepository;
            _validator = validator;
        }

        //Create a new member
        public void CreateMember(Member member)
        {
            _validator.ValidateAndThrow(member);
            member.IsActive = true;
            member.DateCreated = DateTime.UtcNow;

            _memberRepository.Add(member);
            _memberRepository.SaveChanges();
        }

        //Get only active member
        public IEnumerable<Member> GetMembers()
        {
            return _memberRepository
                .GetAll()
                .Where(m => m.IsActive);
        }

        //Get member by ID
        public Member? GetMember(int id)
        {
            return _memberRepository.GetById(id);
        }

        //Update member
        public void UpdateMember(Member member)
        {
            _validator.ValidateAndThrow(member);

            _memberRepository.Update(member);
            _memberRepository.SaveChanges();
        }

        //Soft Delete: Mark IsActive to false
        public void DeleteMember(int id)
        {
            var member = _memberRepository.GetById(id)
                ?? throw new KeyNotFoundException("Member not found.");

            member.IsActive = false;
            _memberRepository.Update(member);
            _memberRepository.SaveChanges();
        }
    }
}
