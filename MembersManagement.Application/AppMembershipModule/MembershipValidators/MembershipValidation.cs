using FluentValidation;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using System;

namespace MembersManagement.Application.AppMembershipModule.MembershipValidators
{
    public class MembershipValidation : AbstractValidator<Membership>
    {
        public MembershipValidation()
        {
            RuleFor(x => x.MembershipName)
            .NotEmpty()
            .WithMessage("Membership Name is required.")
            .MaximumLength(100)
            .WithMessage("Membership Name must not exceed 100 characters.");

        }
    }
}

