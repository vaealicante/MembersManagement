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
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Birthdate cannot be in the future.");

            RuleFor(m => m.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(m => m.Branch)
                .NotEmpty().WithMessage("Branch is required.");

            RuleFor(m => m.ContactNo)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^(09|\+639)\d{9}$")
                .WithMessage("Contact number must be valid.");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
