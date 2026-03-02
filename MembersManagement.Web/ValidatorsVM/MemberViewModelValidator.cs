using FluentValidation;
using MembersManagement.Web.ViewModels;

public class MemberViewModelValidator : AbstractValidator<MemberViewModel>
{
    public MemberViewModelValidator()
    {
        RuleFor(m => m.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(m => m.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(m => m.BirthDate)
            .Must(d => !d.HasValue || d.Value.Date <= DateTime.Today)
            .WithMessage("Birthdate cannot be in the future.")
            .Must(BeAtLeast18)
            .WithMessage("Member must be at least 18 years old.")
            .Must(BeWithinMaxAgeRange)
            .WithMessage("Member cannot be older than 65 years, 6 months, and 1 day.");

        RuleFor(m => m.ContactNo)
                .Matches(@"^(09|\+639)\d{9}$")
                .WithMessage("Contact number must be valid.");

        RuleFor(m => m.Email)
            .EmailAddress()
            .When(m => !string.IsNullOrWhiteSpace(m.Email))
            .WithMessage("Invalid email format.");
    }

    private bool BeAtLeast18(DateTime? birthDate)
    {
        if (!birthDate.HasValue) return true;
        return birthDate.Value.Date <= DateTime.Today.AddYears(-18);
    }

    private bool BeWithinMaxAgeRange(DateTime? birthDate)
    {
        if (!birthDate.HasValue) return true;
        var maxAgeLimit = DateTime.Today.AddYears(-65).AddMonths(-6).AddDays(-1);
        return birthDate.Value.Date >= maxAgeLimit;
    }
}
