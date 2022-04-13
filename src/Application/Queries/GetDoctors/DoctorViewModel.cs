using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.Queries.GetDoctors;

public class DoctorViewModel : IMapFrom<Doctor>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MedicalSpecialization { get; set; }
    public string OfficeLocation { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Doctor, DoctorViewModel>()
            .ForMember(vm => vm.OfficeLocation, opt => opt.MapFrom(d => d.OfficeLocation.GetFullAddress()));
    }
}