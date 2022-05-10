using Api.Dtos.Validators;
using FluentValidation;

namespace Api.Dtos.Medicine;

public class CreateMedicineDtoValidator : AbstractValidator<CreateMedicineDto>
{
    public CreateMedicineDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty()
            .MaximumLength(ValidationConstants.MaxMedicineNameLength)
            .WithMessage("Must contain max " + ValidationConstants.MaxMedicineNameLength + " characters");
        RuleFor(dto => dto.Capacity).NotEmpty()
            .MaximumLength(ValidationConstants.MaxMedicineCapacityLength)
            .WithMessage("Must contain max " + ValidationConstants.MaxMedicineCapacityLength + " characters");
    }
}

public class CreateMedicineDto
{
    public string Name { get; set; }
    public string Capacity { get; set; }
}