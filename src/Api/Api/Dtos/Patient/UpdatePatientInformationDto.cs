using Api.Dtos.Validators;
using FluentValidation;

namespace Api.Dtos.Patient;

public class UpdatePatientInformationDtoValidator : AbstractValidator<UpdatePatientInformationDto>
{
    public UpdatePatientInformationDtoValidator()
    {
        RuleFor(dto => dto.FirstName).MaximumLength(ValidationConstants.MaxFirstNameLength)
            .WithMessage("First name must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.LastName).MaximumLength(ValidationConstants.MaxLastNameLength)
            .WithMessage("Last name must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Email).SetValidator(new EmailAddressValidator())
            .When(dto => !string.IsNullOrEmpty(dto.Email));
        RuleFor(dto => dto.Telephone).MaximumLength(ValidationConstants.MaxTelephoneNumberLength)
            .WithMessage("Telephone must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Pesel).Length(ValidationConstants.ExactPeselLength)
            .WithMessage("Pesel length must be {MaxLength} characters. {TotalLength} characters entered.");
    }
}

public class UpdatePatientInformationDto
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Pesel { get; }

    public UpdatePatientInformationDto(string firstName, string lastName, string email, string telephone, string pesel)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Telephone = telephone;
        Pesel = pesel;
    }
}