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
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            // MemberViewModelValidator.cs
            RuleFor(m => m.BirthDate)
                // Use .HasValue to check if the date was provided
                .Must(date => !date.HasValue || date.Value.Date <= DateTime.Today)
                .WithMessage("Birthdate cannot be in the future.");

            RuleFor(m => m.Address);

            RuleFor(m => m.Branch);

            RuleFor(m => m.ContactNo);

            RuleFor(m => m.Email)
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
