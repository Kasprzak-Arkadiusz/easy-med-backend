using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;
using EasyMed.Domain.Enums;

namespace EasyMed.Application.ViewModels;

public class UserViewModel : IMapFrom<User>
{
    public int Id { get; set; }
    public string Role { get; set; }
    public string EmailAddress { get; set; }
}