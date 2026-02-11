using FluentValidation;
using MembersManagement.Domain.Entities;
using System;

namespace MembersManagement.Application.Validators
{
    public class MemberValidation : AbstractValidator<Member>
    {
        public MemberValidation()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(m => m.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .Must(BeWithinAllowedAgeRange)
            .WithMessage("Age must be between 18 and 65 years, 6 months, and 1 day.");

            RuleFor(m => m.Address);


            RuleFor(m => m.Branch);


            RuleFor(m => m.ContactNo)

                .Matches(@"^(09|\+639)\d{9}$")
                .WithMessage("Contact number must be valid.");

            RuleFor(m => m.Email)

                .EmailAddress().WithMessage("Invalid email format.");
        }

        private bool BeWithinAllowedAgeRange(DateOnly? birthDate)
        {
            // Use .HasValue (Capital H) or != null
            if (birthDate == null) return false;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var dob = birthDate.Value;

            // Minimum Age: 18 years ago from today
            var minDate = today.AddYears(-18);

            // Maximum Age: 65 years, 6 months, and 1 day ago from today
            var maxDate = today.AddYears(-65).AddMonths(-6).AddDays(-1);

            // Validation: 
            // Must be born on or BEFORE minDate (at least 18)
            // Must be born on or AFTER maxDate (not older than 65y 6m 1d)
            return dob <= minDate && dob >= maxDate;
        }
    }
}

