using EasyMed.Domain.Enums;

namespace EasyMed.Application.ViewModels;

public class ReserveVisitViewModel
{
    public int VisitId { get; set; }
    public DateTime VisitDateTime { get; set; }
    public string Location { get; set; }
    public string FullName { get; set; }
    public string MedicalSpecialization { get; set; }
    public bool Completed { get; set; }
}