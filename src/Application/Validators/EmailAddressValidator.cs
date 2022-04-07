using FluentValidation;

namespace EasyMed.Application.Validators;

public class EmailAddressValidator: AbstractValidator<string>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress)
            .MinimumLength(ValidationConstants.MinEmailAddressLength)
            .WithMessage("{PropertyName} must contain at least " + ValidationConstants.MinEmailAddressLength + " characters");

        RuleFor(emailAddress => emailAddress)
            .MaximumLength(ValidationConstants.MaxEmailAddressLength)
            .WithMessage("{PropertyName} must contain max " + ValidationConstants.MaxEmailAddressLength + " characters");
        
        RuleFor(emailAddress => emailAddress)
            .EmailAddress()
            .WithMessage("{PropertyName} has not valid format");
    }
}