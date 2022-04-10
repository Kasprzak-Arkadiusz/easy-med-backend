using FluentValidation;

namespace Api.Dtos.Validators;

public class EmailAddressValidator: AbstractValidator<string>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress)
            .MinimumLength(ValidationConstants.MinEmailAddressLength)
            .WithMessage("Must contain at least " + ValidationConstants.MinEmailAddressLength + " characters");

        RuleFor(emailAddress => emailAddress)
            .MaximumLength(ValidationConstants.MaxEmailAddressLength)
            .WithMessage("Must contain max " + ValidationConstants.MaxEmailAddressLength + " characters");
        
        RuleFor(emailAddress => emailAddress)
            .EmailAddress()
            .WithMessage("Has not valid format");
    }
}