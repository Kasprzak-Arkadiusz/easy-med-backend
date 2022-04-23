using Api.Dtos.Validators;
using FluentValidation;

namespace Api.Dtos.Doctor;

public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(d => d.Rating).GreaterThanOrEqualTo((short)1);
        RuleFor(d => d.Rating).LessThanOrEqualTo((short)5);
        RuleFor(dto => dto.Description).MinimumLength(ValidationConstants.MinDescriptionLength)
            .WithMessage("Description must be less than {MaxLength} characters. {TotalLength} characters entered.");
        RuleFor(dto => dto.Description).MaximumLength(ValidationConstants.MaxDescriptionLength)
            .WithMessage("Description must be less than {MaxLength} characters. {TotalLength} characters entered.");
    }
}

public class CreateReviewDto
{
    public string Description { get; }
    public short Rating { get; }

    public CreateReviewDto(string description, short rating)
    {
        Description = description;
        Rating = rating;
    }
}