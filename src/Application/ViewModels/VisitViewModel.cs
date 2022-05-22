using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class VisitViewModel : IMapFrom<Visit>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DoctorId { get; set; }
    public string Location { get; set; }
    public bool Completed { get; set; }
    public PatientViewModel Patient { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Visit, VisitViewModel>()
            .ForMember(vm => vm.Date, opt => opt.MapFrom(v => v.DateTime))
            .ForMember(vm => vm.Location, opt => opt.MapFrom(v => v.Doctor.OfficeLocation!.Address))
            .ForMember(vm => vm.Completed, opt => opt.MapFrom(v => v.IsCompleted));
    }
}