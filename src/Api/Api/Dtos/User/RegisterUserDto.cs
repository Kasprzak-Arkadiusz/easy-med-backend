using System.ComponentModel.DataAnnotations;
using EasyMed.Domain.Enums;

namespace Api.Dtos.User;

public class RegisterUserDto
{
    [Required]
    public string EmailAddress { get; }
    [Required]
    public string Password { get; }
    [Required]
    public Role RegisterAs { get; }

    public RegisterUserDto(string emailAddress, string password, Role registerAs)
    {
        EmailAddress = emailAddress;
        Password = password;
        RegisterAs = registerAs;
    }
}