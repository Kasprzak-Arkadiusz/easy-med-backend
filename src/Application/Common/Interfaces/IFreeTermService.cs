using EasyMed.Application.Queries.GetDoctors;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.Common.Interfaces;

public interface IFreeTermService
{
    public IEnumerable<FreeTermViewModel> CalculateFreeTerms(DateTime visitDate, Schedule? schedule, IEnumerable<Visit> visits);
}