using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;

namespace EasyMed.Application.Common.Interfaces;

public interface IFreeTermService
{
    IEnumerable<FreeTermViewModel> CalculateFreeTerms(DateTime visitDate, Schedule? schedule, IEnumerable<Visit> visits);
}