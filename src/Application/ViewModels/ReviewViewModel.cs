using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class ReviewViewModel : IMapFrom<Review>
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public short Rating { get; set; }
    public DoctorViewModel Doctor { get; set; }
    public PatientViewModel Patient { get; set; }
}