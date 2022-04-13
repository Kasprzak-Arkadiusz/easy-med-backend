using Api.Dtos.Validators;
using EasyMed.Domain.Enums;
using FluentValidation;

namespace Api.Dtos.User;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty();
        RuleFor(dto => dto.LastName).NotEmpty();
        RuleFor(dto => dto.EmailAddress).SetValidator(new EmailAddressValidator());
        RuleFor(dto => dto.Password).SetValidator(new PasswordValidator());
        RuleFor(dto => dto.RepeatPassword).SetValidator(new PasswordValidator())
            .Equal(dto => dto.Password)
            .WithMessage("Password and repeat password does not match");
        RuleFor(dto => dto.RegisterAs).IsInEnum();
    }
}

public class RegisterUserDto
{
    public string FirstName { get; }
    public string LastName { get; }
    public string EmailAddress { get; }
    public string Password { get; }
    public string RepeatPassword { get; }
    public Role RegisterAs { get; }

    public RegisterUserDto(string firstName, string lastName, string emailAddress, string password, string repeatPassword, Role registerAs)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        RepeatPassword = repeatPassword;
        RegisterAs = registerAs;
    }
}