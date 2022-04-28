using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using DayOfWeek = EasyMed.Domain.Enums.DayOfWeek;

namespace EasyMed.Application.Services;

public class FreeTermService : IFreeTermService
{
    public IEnumerable<FreeTermViewModel> CalculateFreeTerms(DateTime visitDate, Schedule? schedule,
        List<Visit> visits)
    {
        var freeTerms = new List<FreeTermViewModel>();
        if (schedule is null)
        {
            return freeTerms;
        }

        var visitTimeInMinutes = Visit.GetVisitTimeInMinutes();
        var endTime = schedule.EndTime.AddMinutes(-visitTimeInMinutes);
        var isAtLeastOneVisit = visits.Any();

        for (var currentTime = schedule.StartTime;
             currentTime <= endTime;
             currentTime = currentTime.AddMinutes(visitTimeInMinutes))
        {
            bool isFree = true;
            if (isAtLeastOneVisit)
            {
                isFree = !visits.Any(v =>
                    v.DateTime.Hour == currentTime.Hour &&
                    v.DateTime.Minute == currentTime.Minute);
            }

            if (isFree)
            {
                freeTerms.Add(new FreeTermViewModel
                {
                    DayOfWeek = Enum.Parse<DayOfWeek>(schedule.DayOfWeek),
                    VisitDateTime = new DateTime(visitDate.Year, visitDate.Month,
                        visitDate.Day, currentTime.Hour, currentTime.Minute, 0)
                });
            }
        }

        return freeTerms;
    }

    public IEnumerable<DaysWithFreeTermViewModel> GetDaysWithFreeTerm(int daysAhead, List<Schedule> schedules,
        List<Visit> visits)
    {
        var freeDays = new List<DaysWithFreeTermViewModel>();
        var today = DateTime.Today;

        for (int i = 1; i <= daysAhead; i++)
        {
            var thatDay = today.AddDays(i);
            var scheduleThatDay = schedules
                    .FirstOrDefault(s => s.DayOfWeek.ToString() == thatDay.DayOfWeek.ToString());

            if (scheduleThatDay == default)
            {
                continue;
            }

            var numberOfVisitsThatDay = visits.Count(v => v.DateTime.Date == thatDay.Date);
            var workTimeToday = scheduleThatDay.EndTime - scheduleThatDay.StartTime;
            var possibleNumberOfVisitsThatDay = (int) (workTimeToday.TotalMinutes / Visit.GetVisitTimeInMinutes());

            if (numberOfVisitsThatDay != possibleNumberOfVisitsThatDay)
            {
                freeDays.Add(new DaysWithFreeTermViewModel
                {
                    Day = DateOnly.FromDateTime(thatDay),
                    DayOfWeek = Enum.Parse<DayOfWeek>(thatDay.DayOfWeek.ToString())
                });
            }
        }

        return freeDays;
    }
}