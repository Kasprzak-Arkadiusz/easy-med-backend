using Api.Dtos.Validators;
using EasyMed.Domain.Enums;
using FluentValidation;

namespace Api.Dtos.Doctor;

public class UpdateDoctorInformationDtoValidator : AbstractValidator<UpdateDoctorInformationDto>
{
    public UpdateDoctorInformationDtoValidator()
    {
        RuleFor(dto => dto.FirstName).MaximumLength(ValidationConstants.MaxFirstNameLength)
            .WithMessage("First name must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.LastName).MaximumLength(ValidationConstants.MaxLastNameLength)
            .WithMessage("Last name must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Email).SetValidator(new EmailAddressValidator())
            .When(dto => !string.IsNullOrEmpty(dto.Email));
        RuleFor(dto => dto.Telephone).MaximumLength(ValidationConstants.MaxTelephoneNumberLength)
            .WithMessage("Telephone must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Description).MaximumLength(ValidationConstants.MaxDoctorDescriptionLength)
            .WithMessage("Description must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.OfficeLocation).MaximumLength(ValidationConstants.MaxOfficeLocationLength)
            .WithMessage("Office location must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.MedicalSpecialization).IsInEnum();
    }
}

public class UpdateDoctorInformationDto
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Description { get; }
    public string OfficeLocation { get; }
    public MedicalSpecialization? MedicalSpecialization { get; }

    public UpdateDoctorInformationDto(string firstName, string lastName, string email, string telephone,
        string description, string officeLocation, MedicalSpecialization? medicalSpecialization)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Telephone = telephone;
        Description = description;
        OfficeLocation = officeLocation;
        MedicalSpecialization = medicalSpecialization;
    }
}