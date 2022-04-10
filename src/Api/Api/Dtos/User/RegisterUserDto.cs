using Api.Dtos.Validators;
using EasyMed.Domain.Enums;
using FluentValidation;

namespace Api.Dtos.User;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.EmailAddress).NotNull().SetValidator(new EmailAddressValidator());
        RuleFor(dto => dto.Password).NotNull().SetValidator(new PasswordValidator());
        RuleFor(dto => dto.RegisterAs).IsInEnum();
    }
}

public class RegisterUserDto
{
    public string EmailAddress { get; }
    public string Password { get; }
    public Role RegisterAs { get; }

    public RegisterUserDto(string emailAddress, string password, Role registerAs)
    {
        EmailAddress = emailAddress;
        Password = password;
        RegisterAs = registerAs;
    }
}