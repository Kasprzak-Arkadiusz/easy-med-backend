using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class PatientInformationViewModel : IMapFrom<Patient>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string PersonalIdentityNumber { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Patient, PatientInformationViewModel>()
            .ForMember(vm => vm.Email, opt => opt.MapFrom(p => p.EmailAddress))
            .ForMember(vm => vm.Telephone, opt => opt.MapFrom(p => p.TelephoneNumber));
    }
}