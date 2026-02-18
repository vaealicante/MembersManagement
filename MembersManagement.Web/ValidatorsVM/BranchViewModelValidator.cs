using FluentValidation;
using MembersManagement.Web.ViewModels;

namespace MembersManagement.Web.ValidatorsVM
{
    public class BranchViewModelValidator : AbstractValidator<BranchViewModel>
    {
        public BranchViewModelValidator()
        {
            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch Name cannot be empty.")
                .MaximumLength(100).WithMessage("Branch Name must not exceed 100 characters.")
                .Must(BeAValidName).WithMessage("Branch Name contains invalid characters.");

            RuleFor(x => x.Location)
                .MaximumLength(150).WithMessage("Location must not exceed 150 characters.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Status must be specified.");
        }

        // Custom validation logic example
        private bool BeAValidName(string name)
        {
            // Ensures the name isn't just numbers or special symbols
            return !string.IsNullOrWhiteSpace(name) && name.Any(char.IsLetter);
        }
    }
}