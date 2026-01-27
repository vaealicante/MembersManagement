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

            RuleFor(m => m.BirthDate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(date => date.Date <= DateTime.Today)
                .WithMessage("Birthdate cannot be in the future.");

            RuleFor(m => m.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(m => m.Branch)
                .NotEmpty().WithMessage("Branch is required.");

            RuleFor(m => m.ContactNo)
                .NotEmpty().WithMessage("Contact number is required.");

            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
