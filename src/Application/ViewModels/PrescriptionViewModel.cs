using AutoMapper;
using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class PrescriptionViewModel : IMapFrom<Prescription>
{
    public int Id { get; set; }
    public string DateOfIssue { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; }
    public IEnumerable<MedicineViewModel> Medicines { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Prescription, PrescriptionViewModel>()
            .ForMember(vm => vm.DoctorName, opt => opt.MapFrom(p => p.Doctor.GetFullName()));
    }
}