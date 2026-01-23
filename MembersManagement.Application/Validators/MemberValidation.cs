using FluentValidation;
using MembersManagement.Domain.Entities;
using System;

namespace MembersManagement.Application.Validators
{
    public class MemberValidation : AbstractValidator<Member>
    {
        public MemberValidation()
        {
            // Firstname is required
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            // Lastname is required
            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            // Birthdate cannot be in the future
            RuleFor(m => m.BirthDate)
                .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Birthdate cannot be in the future.");
        }
    }
}
