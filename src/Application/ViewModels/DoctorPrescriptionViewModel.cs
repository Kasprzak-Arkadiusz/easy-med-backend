using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class DoctorPrescriptionViewModel : IMapFrom<Prescription>
{
    public int Id { get; set; }
    public DateOnly DateOfIssue { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public IEnumerable<MedicineViewModel> Medicines { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Prescription, DoctorPrescriptionViewModel>()
            .ForMember(vm => vm.PatientName, opt => opt.MapFrom(p => p.Patient.GetFullName()));
    }
}