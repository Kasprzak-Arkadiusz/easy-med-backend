using EasyMed.Application.Common.Mappings;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.ViewModels;

public class MedicineViewModel : IMapFrom<Medicine>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Capacity { get; set; }
}