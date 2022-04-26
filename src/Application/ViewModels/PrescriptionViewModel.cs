using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class PrescriptionViewModel : IMapFrom<Prescription>
{
    public int Id { get; set; }
    public DateOnly DateOfIssue { get; set; }
    public PatientViewModel Patient { get; set; }
    public DoctorViewModel Doctor { get; set; }
    public IEnumerable<MedicineViewModel> Medicines { get; set; }
}