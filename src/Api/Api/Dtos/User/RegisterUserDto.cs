using EasyMed.Domain.Enums;

namespace Api.Dtos.User;

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