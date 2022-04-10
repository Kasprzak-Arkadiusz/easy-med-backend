using Api.Dtos.Validators;
using FluentValidation;

namespace Api.Dtos.User;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(dto => dto.EmailAddress).NotNull().SetValidator(new EmailAddressValidator());
        RuleFor(dto => dto.Password).NotNull().NotEmpty();
    }
}

public class LoginUserDto
{
    public string EmailAddress { get; }
    public string Password { get; }

    public LoginUserDto(string emailAddress, string password)
    {
        EmailAddress = emailAddress;
        Password = password;
    }
}