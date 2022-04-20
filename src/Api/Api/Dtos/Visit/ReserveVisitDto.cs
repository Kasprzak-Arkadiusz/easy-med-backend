using FluentValidation;

namespace Api.Dtos.Visit;

public class ReserveVisitDtoValidator : AbstractValidator<ReserveVisitDto>
{
    public ReserveVisitDtoValidator()
    {
        RuleFor(dto => dto.VisitDateTime).GreaterThan(DateTime.Today).NotNull();
        RuleFor(dto => dto.DoctorId).GreaterThan(0);
        RuleFor(dto => dto.PatientId).GreaterThan(0);
    }
}

public class ReserveVisitDto
{
    public DateTime VisitDateTime { get; }
    public int DoctorId { get; }
    public int PatientId { get; }

    public ReserveVisitDto(DateTime visitDateTime, int doctorId, int patientId)
    {
        VisitDateTime = visitDateTime;
        DoctorId = doctorId;
        PatientId = patientId;
    }
}