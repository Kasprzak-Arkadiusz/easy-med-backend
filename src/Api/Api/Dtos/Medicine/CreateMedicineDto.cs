using FluentValidation;

namespace Api.Dtos.Medicine;

public class CreateMedicineDtoValidator : AbstractValidator<CreateMedicineDto>
{
    public CreateMedicineDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty();
        RuleFor(dto => dto.Capacity).NotEmpty();
    }
}

public class CreateMedicineDto
{
    public string Name { get; set; }
    public string Capacity { get; set; }
}