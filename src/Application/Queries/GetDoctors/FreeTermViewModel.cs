using DayOfWeek = EasyMed.Domain.Enums.DayOfWeek;

namespace EasyMed.Application.Queries.GetDoctors;

public class FreeTermViewModel
{
    public DateTime VisitDateTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}