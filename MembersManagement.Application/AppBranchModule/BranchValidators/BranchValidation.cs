using FluentValidation;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using System;

namespace MembersManagement.Application.AppBranchModule.BranchValidators
{
    public class BranchValidation : AbstractValidator<Branch>
    {
        public BranchValidation()
        {
            // -------------------------------------------------
            // REQUIRED FIELDS
            // -------------------------------------------------

            RuleFor(m => m.BranchName)
                .NotEmpty()
                .WithMessage("Branch name is required.");

        }
    }
}

