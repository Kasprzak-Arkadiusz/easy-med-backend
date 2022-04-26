using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class DoctorInformationViewModel : IMapFrom<Doctor>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Description { get; set; }
    public string OfficeLocation { get; set; }
    public string MedicalSpecialization { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Doctor, DoctorInformationViewModel>()
            .ForMember(vm => vm.Email, opt => opt.MapFrom(d => d.EmailAddress))
            .ForMember(vm => vm.Telephone, opt => opt.MapFrom(d => d.TelephoneNumber))
            .ForMember(vm => vm.OfficeLocation,
                opt => opt.MapFrom(d => d.OfficeLocation == null ? string.Empty : d.OfficeLocation.GetFullAddress()))
            .ForMember(vm => vm.MedicalSpecialization, opt => opt.MapFrom(d => d.MedicalSpecialization));
    }
}