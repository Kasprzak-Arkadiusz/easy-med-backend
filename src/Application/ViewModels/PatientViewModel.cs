using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class PatientViewModel : IMapFrom<Patient>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PersonalIdentityNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? TelephoneNumber { get; set; }
}