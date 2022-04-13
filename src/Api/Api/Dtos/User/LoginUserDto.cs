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
        RuleFor(dto => dto.LoginAs).IsInEnum();
    }
}

public class LoginUserDto
{
    public string EmailAddress { get; }
    public string Password { get; }
    public Role LoginAs { get; }

    public LoginUserDto(string emailAddress, string password, Role loginAs)
    {
        EmailAddress = emailAddress;
        Password = password;
        LoginAs = loginAs;
    }
}