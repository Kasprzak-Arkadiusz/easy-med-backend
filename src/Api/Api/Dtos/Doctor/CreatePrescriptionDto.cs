using Api.Dtos.Medicine;
using FluentValidation;

namespace Api.Dtos.Doctor;

public class CreatePrescriptionDtoValidator : AbstractValidator<CreatePrescriptionDto>
{
    public CreatePrescriptionDtoValidator()
    {
        RuleFor(dto => dto.PatientId).GreaterThanOrEqualTo(1);
        RuleFor(dto => dto.Medicines).NotEmpty();
        RuleForEach(dto => dto.Medicines).SetValidator(new CreateMedicineDtoValidator());
    }
}

public class CreatePrescriptionDto
{
    public int PatientId { get; set; }
    public IEnumerable<CreateMedicineDto> Medicines { get; set; }
    
}