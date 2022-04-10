using FluentValidation;

namespace Api.Dtos.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .MinimumLength(ValidationConstants.MinPasswordLength)
            .WithMessage("Must contain at least " + ValidationConstants.MinPasswordLength + " characters");

        RuleFor(password => password)
            .MaximumLength(ValidationConstants.MaxPasswordLength)
            .WithMessage("Must contain max " + ValidationConstants.MaxPasswordLength + " characters");

        RuleFor(password => password)
            .Matches(@"[0-9]+")
            .WithMessage("Must contain at least one number");

        RuleFor(password => password)
            .Matches(@"[A-Z]+")
            .WithMessage("Must contain at least one uppercase character");

        RuleFor(password => password)
            .Matches(@"[a-z]+")
            .WithMessage("Must contain at least one uppercase character");
    }
}