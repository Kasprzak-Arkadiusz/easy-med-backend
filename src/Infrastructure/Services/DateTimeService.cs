using EasyMed.Application.Common.Interfaces;

namespace EasyMed.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}