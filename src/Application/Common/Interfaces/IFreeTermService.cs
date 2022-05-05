using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.Common.Interfaces;

public interface IFreeTermService
{
    IEnumerable<FreeTermViewModel>
        CalculateFreeTerms(DateTime visitDate, Schedule? schedule, List<Visit> visits);

    IEnumerable<DaysWithFreeTermViewModel> GetDaysWithFreeTerm(int daysAhead, List<Schedule> schedules,
        List<Visit> visits);
}