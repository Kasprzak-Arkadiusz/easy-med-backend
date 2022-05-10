using EasyMed.Application.ViewModels;

namespace Api.Dtos.Doctor;

public class CreatePrescriptionDto
{
    public int PatientId { get; set; }
    public IEnumerable<CreateMedicineViewModel> Medicines { get; set; }
    
}