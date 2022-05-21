using EasyMed.Application.Commands;
using FluentValidation;

namespace Api.Dtos.Doctor;

public class AddAvailabilitiesDtoValidator : AbstractValidator<AddAvailabilitiesDto>
{
    public AddAvailabilitiesDtoValidator()
    {
        RuleFor(dto => dto.Availablities).NotNull().NotEmpty();
        RuleForEach(dto => dto.Availablities).NotNull().NotEmpty();
    }
}

public class AddAvailabilitiesDto
{
    public IEnumerable<Availability> Availablities { get; set; }
}