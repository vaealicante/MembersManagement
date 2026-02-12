using FluentValidation;
using MembersManagement.Web.ViewModels;
using System;

namespace MembersManagement.Web.ValidatorsVM
{
    public class MemberViewModelValidator : AbstractValidator<MemberViewModel>
    {
        public MemberViewModelValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First Name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            RuleFor(m => m.BirthDate) 
                .Must(date => !date.HasValue || date.Value.Date <= DateTime.Today)
                .WithMessage("Birthdate cannot be in the future.")
                .Must(BeAtLeast18)
                .WithMessage("Member must be at least 18 years old.")
                .Must(BeWithinMaxAgeRange)
                .WithMessage("Member cannot be older than 65 years, 6 months, and 1 day.");

            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Invalid email format.");

        }

        // Helper: Check if user is at least 18 years old
        private bool BeAtLeast18(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return true; // Handled by NotEmpty
            var today = DateTime.Today;
            return birthDate.Value.Date <= today.AddYears(-18);
        }

        // Helper: Check if user is within the 65y 6m 1d limit
        private bool BeWithinMaxAgeRange(DateTime? birthDate)
        {
            if (!birthDate.HasValue) return true; // Handled by NotEmpty
            var today = DateTime.Today;

            // Earliest possible valid birthdate
            var maxAgeLimit = today.AddYears(-65).AddMonths(-6).AddDays(-1);

            return birthDate.Value.Date >= maxAgeLimit;
        }
    }
}