using EasyMed.Application.Commands;
using FluentValidation;

namespace Api.Dtos.Doctor;

public class AddAvailabilitiesDtoValidator : AbstractValidator<AddAvailabilitiesDto>
{
    public AddAvailabilitiesDtoValidator()
    {
        RuleFor(dto => dto.Availabilities).NotNull().NotEmpty();
        RuleForEach(dto => dto.Availabilities).NotNull().NotEmpty();
    }
}

public class AddAvailabilitiesDto
{
    public IEnumerable<Availability> Availabilities { get; set; }
}