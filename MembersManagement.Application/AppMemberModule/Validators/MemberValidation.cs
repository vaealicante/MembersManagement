using FluentValidation;
using MembersManagement.Domain.DomMemberModule.Entities;
using System;

namespace MembersManagement.Application.AppMemberModule.Validators
{
    /// Defines validation rules for the Member entity.
    /// Ensures required fields are present and optional fields
    /// follow business constraints when provided.
    public class MemberValidation : AbstractValidator<Member>
    {
        public MemberValidation()
        {
            // -------------------------------------------------
            // REQUIRED FIELDS
            // -------------------------------------------------

            // Ensures the member has a first name.
            RuleFor(m => m.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.");

            // Ensures the member has a last name.
            RuleFor(m => m.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");

            // -------------------------------------------------
            // OPTIONAL BIRTHDATE
            // -------------------------------------------------
            // Applied only when BirthDate has a value.
            // Validates:
            // 1. Birthdate is not in the future
            // 2. Member is at least 18 years old
            // 3. Member does not exceed the maximum allowed age
            RuleFor(m => m.BirthDate)
                .Cascade(CascadeMode.Stop) // Stop further checks if one rule fails
                .Must(NotBeInFuture)
                .WithMessage("Birthdate cannot be in the future.")
                .Must(BeAtLeast18)
                .WithMessage("Member must be at least 18 years old.")
                .Must(NotExceedMaxAge)
                .WithMessage("Member cannot be older than 65 years, 6 months, and 1 day.")
                .When(m => m.BirthDate.HasValue);

            // -------------------------------------------------
            // OPTIONAL CONTACT NUMBER
            // -------------------------------------------------
            // Validates Philippine mobile number format:
            // Starts with 09 or +639 and contains 11 digits total.
            RuleFor(m => m.ContactNo)
                .Matches(@"^(09|\+639|639)\d{9}$")
                .WithMessage("Contact number must be valid.")
                .When(m => !string.IsNullOrWhiteSpace(m.ContactNo));

            // -------------------------------------------------
            // OPTIONAL EMAIL
            // -------------------------------------------------
            // Ensures the email is in a valid format if provided.
            RuleFor(m => m.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .When(m => !string.IsNullOrWhiteSpace(m.Email));
        }

        /// Ensures the birthdate is not set to a future date.
        private bool NotBeInFuture(DateOnly? date) =>
            date!.Value <= DateOnly.FromDateTime(DateTime.Today);

        /// Ensures the member is at least 18 years old.
        private bool BeAtLeast18(DateOnly? date) =>
            date!.Value <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18);

        /// Ensures the member does not exceed the maximum age
        /// of 65 years, 6 months, and 1 day.
        private bool NotExceedMaxAge(DateOnly? date) =>
            date!.Value >= DateOnly.FromDateTime(DateTime.Today)
                .AddYears(-65)
                .AddMonths(-6)
                .AddDays(-1);
    }
}
