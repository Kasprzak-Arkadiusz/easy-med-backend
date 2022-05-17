using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class ReviewDoctorViewModel : IMapFrom<Doctor>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MedicalSpecialization { get; set; }
}