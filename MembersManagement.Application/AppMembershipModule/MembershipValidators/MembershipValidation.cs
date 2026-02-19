using FluentValidation;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using System;

namespace MembersManagement.Application.AppMembershipModule.MembershipValidators
{
    public class MembershipValidation : AbstractValidator<Membership>
    {
        public MembershipValidation()
        {
            // -------------------------------------------------
            // REQUIRED FIELDS
            // -------------------------------------------------

            RuleFor(m => m.MembershipName)
                .NotEmpty()
                .WithMessage("Membership name is required.");

        }
    }
}

