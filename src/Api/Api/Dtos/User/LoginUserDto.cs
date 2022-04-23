using Api.Dtos.Validators;
using EasyMed.Domain.Enums;
using FluentValidation;

namespace Api.Dtos.User;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(dto => dto.EmailAddress).SetValidator(new EmailAddressValidator());
        RuleFor(dto => dto.Password).NotEmpty();
        RuleFor(dto => dto.Role).IsInEnum();
    }
}

public class LoginUserDto
{
    public string EmailAddress { get; }
    public string Password { get; }
    public Role Role { get; }

    public LoginUserDto(string emailAddress, string password, Role role)
    {
        EmailAddress = emailAddress;
        Password = password;
        Role = role;
    }
}