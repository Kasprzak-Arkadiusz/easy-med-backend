using FluentValidation;

namespace Api.Dtos.Doctor;

public class AddAvailabilityDtoValidator : AbstractValidator<AddAvailabilityDto>
{
    public AddAvailabilityDtoValidator()
    {
        RuleFor(dto => dto.StartDate).NotNull().NotEmpty();
        RuleFor(dto => dto.EndDate).NotNull().NotEmpty();
    }
}

public class AddAvailabilityDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}