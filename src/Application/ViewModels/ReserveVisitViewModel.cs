using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class ReserveVisitViewModel : IMapFrom<Visit>
{
    public int VisitId { get; set; }
    public DateTime VisitDateTime { get; set; }
    public string Location { get; set; }
    public string FullName { get; set; }
    public string MedicalSpecialization { get; set; }
    public bool Completed { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Visit, ReserveVisitViewModel>()
            .ForMember(vm => vm.VisitId, opt => opt.MapFrom(v => v.Id))
            .ForMember(vm => vm.VisitDateTime, opt => opt.MapFrom(v => v.DateTime))
            .ForMember(vm => vm.Location, opt => opt.MapFrom(v => v.Doctor.OfficeLocation!.Address))
            .ForMember(vm => vm.FullName, opt => opt.MapFrom(v => v.Doctor.GetFullName()))
            .ForMember(vm => vm.MedicalSpecialization, opt => opt.MapFrom(v => v.Doctor.MedicalSpecialization))
            .ForMember(vm => vm.Completed, opt => opt.MapFrom(v => v.IsCompleted));
    }
}