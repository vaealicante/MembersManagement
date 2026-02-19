using FluentValidation;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Application.AppMembershipModule.MembershipBusinessLogic
{
    public class MembershipManager(
    IMembershipRepository membershipRepository,
    IValidator<Membership> validator)
    {
        private readonly IMembershipRepository _membershipRepository = membershipRepository;
        private readonly IValidator<Membership> _validator = validator;

        public void CreateMembership (Membership membership)
        {
           
            _validator.ValidateAndThrow(membership);

            membership.IsActive = true;
            membership.DateCreated = DateTime.UtcNow;

            _membershipRepository.Add(membership);
            _membershipRepository.SaveChanges();
        }


        public IEnumerable<Membership> GetMembership()
        {
            return _membershipRepository
                .GetAll()
                .Where(m => m.IsActive);
        }

        public Membership? GetMembershipById(int id)
        {
            return _membershipRepository.GetById(id);
        }


        public void UpdateMembership(Membership membership)
        {
            // Re-validates age range and format rules if data was changed
            _validator.ValidateAndThrow(membership);

            _membershipRepository.Update(membership);
            _membershipRepository.SaveChanges();
        }

        /// <summary>
        /// Performs a Soft Delete by switching the IsActive flag.
        /// </summary>
        public void DeleteMembership(int id)
        {
            var membership = _membershipRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Membership with ID {id} was not found.");

            membership.IsActive = false;

            _membershipRepository.Update(membership);
            _membershipRepository.SaveChanges();
        }

        /// <summary>
        /// Administrative method to see all records regardless of status.
        /// </summary>
        public IEnumerable<Membership> GetAllMembershipsRaw()
        {
            return _membershipRepository.GetAll();
        }

    }
}