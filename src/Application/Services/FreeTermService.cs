using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.Services;

public class FreeTermService : IFreeTermService
{
    public IEnumerable<FreeTermViewModel> CalculateFreeTerms(DateTime visitDate, Schedule? schedule,
        IEnumerable<Visit> visits)
    {
        var freeTerms = new List<FreeTermViewModel>();
        if (schedule is null)
        {
            return freeTerms;
        }
        
        var visitTimeInMinutes = Visit.GetVisitTimeInMinutes();
        var endTime = schedule.EndDate.AddMinutes(-visitTimeInMinutes);
        var visitList = visits.ToList();
        var isAtLeastOneVisit = visitList.Any();

        for (var currentTime = schedule.StartDate; currentTime <= endTime; currentTime = currentTime.AddMinutes(visitTimeInMinutes))
        {
            bool isFree = true;
            if (isAtLeastOneVisit)
            {
                isFree = !visitList.Any(v =>
                    v.DateTime.Hour == currentTime.Hour &&
                    v.DateTime.Minute == currentTime.Minute);
            }

            if (isFree)
            {
                freeTerms.Add(new FreeTermViewModel
                {
                    DayOfWeek = schedule.StartDate.DayOfWeek,
                    VisitDateTime = new DateTime(visitDate.Year, visitDate.Month,
                        visitDate.Day, currentTime.Hour, currentTime.Minute, 0)
                });
            }
        }

        return freeTerms;
    }
}