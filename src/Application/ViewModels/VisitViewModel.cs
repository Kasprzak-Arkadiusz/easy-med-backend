using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class VisitViewModel : IMapFrom<Visit>
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsCompleted { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public PatientViewModel Patient { get; set; }
}