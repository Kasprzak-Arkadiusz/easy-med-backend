using FluentValidation;

namespace EasyMed.Application.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .MinimumLength(ValidationConstants.MinPasswordLength)
            .WithMessage("{PropertyName} must contain at least " + ValidationConstants.MinPasswordLength + " characters");

        RuleFor(password => password)
            .MaximumLength(ValidationConstants.MaxPasswordLength)
            .WithMessage("{PropertyName} must contain max " + ValidationConstants.MaxPasswordLength + " characters");

        RuleFor(password => password)
            .Matches(@"[0-9]+")
            .WithMessage("{PropertyName} must contain at least one number");

        RuleFor(password => password)
            .Matches(@"[A-Z]+")
            .WithMessage("{PropertyName} must contain at least one uppercase character");

        RuleFor(password => password)
            .Matches(@"[a-z]+")
            .WithMessage("{PropertyName} must contain at least one uppercase character");
    }
}