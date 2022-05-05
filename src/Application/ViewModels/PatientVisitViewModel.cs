using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class PatientVisitViewModel : IMapFrom<Visit>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string DoctorName { get; set; }
    public string MedicalSpecialization { get; set; }
    public bool Completed { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Visit, PatientVisitViewModel>()
            .ForMember(vm => vm.Date, opt => opt.MapFrom(v => v.DateTime))
            .ForMember(vm => vm.Location, opt => opt.MapFrom(v => v.Doctor.OfficeLocation!.Address))
            .ForMember(vm => vm.DoctorName, opt => opt.MapFrom(v => v.Doctor.GetFullName()))
            .ForMember(vm => vm.MedicalSpecialization, opt => opt.MapFrom(v => v.Doctor.MedicalSpecialization))
            .ForMember(vm => vm.Completed, opt => opt.MapFrom(v => v.IsCompleted));
    }
}