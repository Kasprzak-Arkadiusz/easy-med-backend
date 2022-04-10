using EasyMed.Application.Common.Mappings;

namespace EasyMed.Application.ViewModels;

public class AuthViewModel
{
    public string AccessToken { get; set; }
    public string EmailAddress { get; set; }
    public string Role { get; set; }
}