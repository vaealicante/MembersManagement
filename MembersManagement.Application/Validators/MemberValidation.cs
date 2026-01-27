using FluentValidation;
using MembersManagement.Domain.Entities;
using System;

namespace MembersManagement.Application.Validators
{
    public class MemberValidation : AbstractValidator<Member>
    {
        public MemberValidation()
        {
            // First Name required
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            // Last Name required
            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            // Birthdate required and cannot be in the future
            RuleFor(m => m.BirthDate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Birthdate cannot be in the future.");

            // Address required
            RuleFor(m => m.Address)
                .NotEmpty().WithMessage("Address is required.");

            // Branch required
            RuleFor(m => m.Branch)
                .NotEmpty().WithMessage("Branch is required.");

            // ContactNo required and must be a valid phone number
            RuleFor(m => m.ContactNo)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^\+?\d{7,15}$")
                .WithMessage("Contact number must be valid.");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // IsActive - optional, default true, can skip validation
        }
    }
}
